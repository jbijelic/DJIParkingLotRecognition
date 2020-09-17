using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectDetectionAPI.FasterRCNN
{
    public class CustomVisionPrediction : IOnnxObjectPrediction
    {
        [ColumnName("model_outputs0")]
        public float[] PredictedLabels { get; set; }
    }
}
