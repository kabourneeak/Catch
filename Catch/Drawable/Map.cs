using System.Collections.Generic;
using Catch.Base;
using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Catch.Drawable
{
    public class Map : BasicMap, IDrawable
    {
        protected override sealed IHexTileProvider TileProvider { get; set; }

        public Map(IHexTileProvider provider)
        {
            TileProvider = provider;
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            foreach (var tile in Tiles)
                ((IDrawable)tile).Draw(drawingSession);
        }

        // TODO where should we store the radius?  we can't put "60" everywhere!
        public Vector2 SizeInPixels { get { return new Vector2((float)(Columns * 60 * 1.5 + 30), (float)(Rows * 2 * 60 * Hexagon.SIN60)); } }
    }
}
