using Windows.UI;
using Microsoft.Graphics.Canvas;

namespace Catch.Base
{
    public class StyleArgs
    {
        public Color Color { get; set; }
        public ICanvasBrush Brush { get; set; }
        public CanvasStrokeStyle StrokeStyle { get; set; }
        public int StrokeWidth { get; set; }
    }
}
