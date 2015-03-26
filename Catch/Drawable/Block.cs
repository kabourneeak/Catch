using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Catch.Base;
using Catch.Models;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Drawable
{
    public class Block : BasicMob, IMovable, IUpdatable, IDrawable
    {
        private IConfig _config;
        private readonly int _blockSize;

        public Vector2 Position { get; set; }

        public class ConfigKeys
        {
            public static readonly string BlockSize = typeof(Block).FullName + ".BlockSize";
        }

        public Block(IConfig config, IPath path) : base(path)
        {
            _config = config;

            _blockSize = _config.GetInt(ConfigKeys.BlockSize);
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            var tile = (Hexagon) this.Tile;

            drawingSession.FillRectangle(new Rect(tile.CenterPoint.X - _blockSize / 2, tile.CenterPoint.Y - _blockSize / 2, _blockSize, _blockSize), Colors.Yellow);
        }

        public new void Update(int ticks)
        {
            base.Update(ticks);
        }

    }
}
