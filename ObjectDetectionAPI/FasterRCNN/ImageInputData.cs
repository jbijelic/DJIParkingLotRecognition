using Microsoft.ML.Transforms.Image;
using System.Drawing;

namespace ObjectDetectionAPI.FasterRCNN
{
    public class ImageInputData
    {
        [ImageType(416, 416)]
        public Bitmap Image { get; set; }
    }
}
