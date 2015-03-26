using System.Numerics;
using Catch.Base;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Drawable
{
    public class Map : BasicMap, IDrawable
    {
        public class ConfigKeys
        {
            public static readonly string TileRadius = typeof(Hexagon).FullName + ".TileRadius";
        }

        private readonly float _tileRadius;

        public Map(IConfig config, IHexTileProvider tileProvider) : base(tileProvider)
        {
            _tileRadius = config.GetFloat(ConfigKeys.TileRadius);
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            foreach (var tile in Tiles)
                ((IDrawable)tile).Draw(drawingSession);
        }

        public Vector2 SizeInPixels { get { return new Vector2((float)(Columns * _tileRadius * 1.5 + _tileRadius / 2), (float)(Rows * 2 * Hexagon.GetRadiusHeight(_tileRadius))); } }
    }
}
