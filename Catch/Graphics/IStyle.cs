using Windows.UI;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Graphics
{
    public interface IStyle
    {
        string Name { get; }
        
        Color Color { get; }

        ICanvasBrush Brush { get; }

        CanvasStrokeStyle StrokeStyle { get; }

        float StrokeWidth { get; }
    }
}
