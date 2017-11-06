using Windows.UI;
using Microsoft.Graphics.Canvas.Brushes;

namespace Catch.Graphics
{
    public interface IStyle : IGraphicsResource
    {
        string Name { get; }
        
        Color Color { get; }

        ICanvasBrush Brush { get; }

        float StrokeWidth { get; }
    }
}
