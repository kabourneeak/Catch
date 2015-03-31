using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Catch.Base;
using Catch.Models;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Win2d
{
    class BlockMobGraphics : IGraphicsComponent
    {
        private readonly PathMobAgent _agent;
        private IConfig _config;
        private readonly float _blockSize;
        private readonly Color _blockColour;

        public BlockMobGraphics(PathMobAgent agent, IConfig config)
        {
            _agent = agent;
            _config = config;

            _blockSize = 10;
            _blockColour = Colors.Yellow;
        }

        public Vector2 Position { get; private set; }

        public DrawLayer Layer { get; private set; }

        public void Update(float ticks)
        {
            // do nothing
        }

        public void CreateResources(CanvasDrawingSession drawingSession)
        {
            // do nothing
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            var tilePos = _agent.Tile.Graphics.Position;

            drawingSession.FillRectangle(new Rect(tilePos.X - _blockSize / 2, tilePos.Y - _blockSize / 2, _blockSize,
                    _blockSize), _blockColour);
        }
    }
}
