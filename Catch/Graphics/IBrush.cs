using Microsoft.Graphics.Canvas.Brushes;

namespace Catch.Graphics
{
    public interface IBrush
    {
        string Name { get; }
        
        ICanvasBrush Brush { get; }
    }
}
