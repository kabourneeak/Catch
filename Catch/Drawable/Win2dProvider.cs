using Catch.Base;
using Catch.Models;

namespace Catch.Drawable
{
    public class Win2DProvider : IHexTileProvider
    {
        public IHexTile CreateTile(int row, int col)
        {
            return new Hexagon(row, col);
        }
    }
}
