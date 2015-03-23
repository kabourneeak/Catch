using Microsoft.Graphics.Canvas;

namespace Catch.Drawable
{
    public interface IDrawable
    {
        void Draw(CanvasDrawingSession drawingSession);
    }
}
