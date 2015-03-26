using Catch.Base;
using Catch.Models;

namespace Catch.Drawable
{
    public class Win2DProvider : IHexTileProvider
    {
        private float TileRadius { get; set; }

        public Win2DProvider(float tileRadius)
        {
            TileRadius = tileRadius;
        }

        public IHexTile CreateTile(int row, int col)
        {
            return new Hexagon(row, col, TileRadius);
        }
    }
}
