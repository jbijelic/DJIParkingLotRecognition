using Microsoft.ML.Data;

namespace ObjectDetectionAPI.FasterRCNN
{
    public class FasterRCNNPrediction : IOnnxObjectPrediction
    {
        [ColumnName("grid")]
        public float[] PredictedLabels { get; set; }
    }
}
