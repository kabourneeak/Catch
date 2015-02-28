using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class Block : IMovable
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        private const int Size = 20;

        public void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.FillRectangle(new Rect(Position.X, Position.Y, Size, Size), Colors.Red);
        }

        public void Update()
        {
            Velocity = Vector2.Add(Velocity, Acceleration);
            Position = Vector2.Add(Position, Velocity);
        }
    }
}
