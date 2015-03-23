using Catch.Models;

namespace Catch.Base
{
    public interface IHexTileProvider
    {
        IHexTile CreateTile(int row, int col);
    }
}