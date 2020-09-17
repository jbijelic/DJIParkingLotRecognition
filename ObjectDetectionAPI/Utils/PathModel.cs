using System.IO;

namespace ObjectDetectionAPI.Utils
{
    public class PathModel
    {
        public string AssetsRelativePath { get; set; }
        public string AssetsPath { get; set; }
        public string ModelFilePath { get; set; }
        public string OutputFolder { get; set; }

        public PathModel()
        {
            AssetsRelativePath = @"../../../Debug/AppX/Assets";
            AssetsPath = Path.GetFullPath(AssetsRelativePath);
            ModelFilePath = Path.Combine(AssetsPath, "model", "fasterRCNN.onnx");
        }
    }
}
