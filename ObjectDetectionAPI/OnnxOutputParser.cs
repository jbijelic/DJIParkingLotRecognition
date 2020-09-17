using ObjectDetectionAPI.FasterRCNN;
using ObjectDetectionAPI.Utils;
using Microsoft.ML;
using System.Collections.Generic;

namespace ObjectDetectionAPI
{
    public class OnnxOutputParser
    {
        public PathModel PathModel { get; set; }

        public struct ImageNetSettings
        {
            public const int imageHeight = 416;
            public const int imageWidth = 416;
        }

        private struct InceptionSettings
        {
            public const int ImageHeight = 224;
            public const int ImageWidth = 224;
            public const float Mean = 117;
            public const float Scale = 1;
            public const bool ChannelsLast = true;
        }

        public struct ImageSettings
        {
            public const int imageHeight = 416;
            public const int imageWidth = 416;
            public const float mean = 117;         //offsetImage
            public const bool channelsLast = true; //interleavePixelColors
        }

        public struct TensorFlowModelSettings
        {
            // input tensor name
            public const string inputTensorName = "image_tensor";

            // output tensor name
            public const string outputTensorName = "loss";
        }

        public struct TinyFasterRCNNModelSettings
        {
            // for checking Tiny yolo2 Model input and  output  parameter names,
            //you can use tools like Netron, 
            // which is installed by Visual Studio AI Tools

            // input tensor name
            public const string ModelInput = "image";

            // output tensor name
            public const string ModelOutput = "grid";
        }

        private readonly string modelLocation;
        private readonly MLContext mlContext;

        private IList<FasterRCNNBoundingBox> _boundingBoxes = new List<FasterRCNNBoundingBox>();
    }
}
