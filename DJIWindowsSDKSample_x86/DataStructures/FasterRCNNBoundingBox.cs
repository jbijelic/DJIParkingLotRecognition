using System.Drawing;

namespace DJIWindowsSDKSample.DataStructures
{
    public class BoundingBoxDimensions : DimensionsBase { }
    public class FasterRCNNBoundingBox
    {
        public struct ImageSettings
        {
            public const int imageHeight = 416;
            public const int imageWidth = 416;
            public const float mean = 117;
            public const bool channelsLast = true; 
        }

        public BoundingBoxDimensions Dimensions { get; set; }

        public string Label { get; set; }

        public float Confidence { get; set; }

        public RectangleF Rect
        {
            get { return new RectangleF(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height); }
        }

        public Color BoxColor { get; set; }
    }
}
