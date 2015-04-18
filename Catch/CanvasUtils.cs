using System.Numerics;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch
{
    public static class CanvasUtils
    {
        public static Matrix3x2 GetDisplayTransform(Size controlSize, ICanvasResourceCreatorWithDpi canvas, int designWidth, int designHeight)
        {
            // Scale the display to fill the control.
            var canvasSize = controlSize.ToVector2();
            var simulationSize = new Vector2(canvas.ConvertPixelsToDips(designWidth), canvas.ConvertPixelsToDips(designHeight));
            var scale = canvasSize / simulationSize;
            var offset = Vector2.Zero;

            // Letterbox or pillarbox to preserve aspect ratio.
            if (scale.X > scale.Y)
            {
                scale.X = scale.Y;
                offset.X = (canvasSize.X - simulationSize.X * scale.X) / 2;
            }
            else
            {
                scale.Y = scale.X;
                offset.Y = (canvasSize.Y - simulationSize.Y * scale.Y) / 2;
            }

            return Matrix3x2.CreateScale(scale) * Matrix3x2.CreateTranslation(offset);
        }

        public static CanvasGeometry CreateStarGeometry(ICanvasResourceCreator resourceCreator, float scale, Vector2 center)
        {
            var pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(new Vector2(-0.24f, -0.24f) * scale + center);
            pathBuilder.AddLine(new Vector2(0, -1) * scale + center);
            pathBuilder.AddLine(new Vector2(0.24f, -0.24f) * scale + center);
            pathBuilder.AddLine(new Vector2(1, -0.2f) * scale + center);
            pathBuilder.AddLine(new Vector2(0.4f, 0.2f) * scale + center);
            pathBuilder.AddLine(new Vector2(0.6f, 1) * scale + center);
            pathBuilder.AddLine(new Vector2(0, 0.56f) * scale + center);
            pathBuilder.AddLine(new Vector2(-0.6f, 1) * scale + center);
            pathBuilder.AddLine(new Vector2(-0.4f, 0.2f) * scale + center);
            pathBuilder.AddLine(new Vector2(-1, -0.2f) * scale + center);
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            return CanvasGeometry.CreatePath(pathBuilder);
        }
    }
}
