using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Catch.Models;
using Microsoft.Graphics.Canvas;

namespace Catch.Drawable
{
    public class Block : IMovable, IUpdatable, IDrawable
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

        public void Update(int ticks)
        {
            Velocity = Vector2.Add(Velocity, Acceleration);
            Position = Vector2.Add(Position, Velocity);
        }

        public IPath Path
        {
            get { throw new System.NotImplementedException(); }
        }

        public IHexTile Tile
        {
            get { throw new System.NotImplementedException(); }
        }

        public float TileProgress
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
