using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Catch.Models
{
    public class Block : IMovable
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        private const int _blockSize = 20;
        public static int Size { get { return _blockSize; } } 

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
