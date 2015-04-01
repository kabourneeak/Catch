using System.Numerics;
using Catch.Base;
using Catch.Models;
using Microsoft.Graphics.Canvas;

namespace Catch.Win2d
{
    public class MapGraphics : IGraphicsComponent
    {
        private readonly BasicMap _map;
        private int _tileRadius;

        public MapGraphics(BasicMap map)
        {
            _map = map;

            _tileRadius = 60;

            // set basic properties
            Position = new Vector2();
            Layer = DrawLayer.Base;
        }

        #region IGraphicsComponent implementation

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
            foreach (var tile in _map.Tiles)
            {
                tile.Graphics.Draw(drawingSession);
            }
        }

        #endregion

        public Vector2 SizeInPixels { get { return new Vector2((float)(_map.Columns * _tileRadius * 1.5 + _tileRadius / 2), (float)(_map.Rows * 2 * BasicHexTileGraphics.GetRadiusHeight(_tileRadius))); } }

    }
}
