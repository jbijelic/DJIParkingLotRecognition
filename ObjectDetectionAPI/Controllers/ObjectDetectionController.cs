using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Transforms.Image;
using ObjectDetectionAPI.FasterRCNN;
using static ObjectDetectionAPI.OnnxOutputParser;

namespace ObjectDetectionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObjectDetectionController : ControllerBase
    {

        private PredictionEngine<ImageInputData, FasterRCNNPrediction> fasterRCNNPredictionEngine;
        public MLContext ctx { get; set; }
        private FasterRCNNOutputParser outputParser;

        public ObjectDetectionController()
        {
            ctx = new MLContext();

            // Define model path
            var modelPath = Path.Join(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "FasterRCNN-10.onnx");

            var fasterRCNNModel = new FasterRCNNModel(modelPath);

            //Load model
            ITransformer mlModel = SetupMlNetModel(fasterRCNNModel);//ctx.Model.Load(modelPath, out var modelInputSchema);

            // Create prediction engine
            this.fasterRCNNPredictionEngine = ctx.Model.CreatePredictionEngine<ImageInputData, FasterRCNNPrediction>(mlModel);

            outputParser = new FasterRCNNOutputParser(fasterRCNNModel);
        }

        [HttpPost]
        public async Task<List<BoundingBox>> ClassifyImage([FromBody] Dictionary<string, string> input)
        {
            Bitmap bitmap;

            // Get raw image bytes
            var imageBytes = Convert.FromBase64String(input["data"]);

            using (var ms = new MemoryStream(imageBytes))
            {
                bitmap = new Bitmap(ms);
            }

            return await ParseDroneCameraFrame(bitmap, 0, 0);            
        }

        private async Task<List<BoundingBox>> ParseDroneCameraFrame(Bitmap stream, int width, int height)
        {
            if (fasterRCNNPredictionEngine == null)
                return null;


            var frame = new ImageInputData { Image = stream };

            var filteredBoxes = DetectObjectsUsingModel(frame);

            return filteredBoxes;
        }

        private List<BoundingBox> DetectObjectsUsingModel(ImageInputData imageInputData)
        {
            var labels = fasterRCNNPredictionEngine?.Predict(imageInputData).PredictedLabels ?? fasterRCNNPredictionEngine?.Predict(imageInputData).PredictedLabels;
            var boundingBoxes = outputParser.ParseOutputs(labels);
            var filteredBoxes = outputParser.FilterBoundingBoxes(boundingBoxes, 5, 0.5f);
            return filteredBoxes;
        }

        private ITransformer SetupMlNetModel(IOnnxModel onnxModel)
        {
            SessionOptions opts = new SessionOptions();

            var dataView = ctx.Data.LoadFromEnumerable(new List<ImageInputData>());

            var pipeline = ctx.Transforms.ResizeImages(resizing: ImageResizingEstimator.ResizingKind.Fill, outputColumnName: onnxModel.ModelInput, imageWidth: ImageSettings.imageWidth, imageHeight: ImageSettings.imageHeight, inputColumnName: nameof(ImageInputData.Image))
            .Append(ctx.Transforms.ExtractPixels(outputColumnName: onnxModel.ModelInput))
            .Append(ctx.Transforms.ApplyOnnxModel(modelFile: onnxModel.ModelPath, outputColumnName: "6379", inputColumnName: onnxModel.ModelInput));

            var mlNetModel = pipeline.Fit(dataView);

            return mlNetModel;
        }


    }
}
