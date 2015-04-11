using System.Collections.Generic;
using Catch.Models;

namespace Catch.Base
{
    /// <summary>
    /// Provides information about a hex tile grid, which has tiles, towers, 
    /// paths. A grid is always a full rectangle -- there are no missing 
    /// instances in the middle.  However, the grid is not a proper rectangle
    /// owing the hex shape. The even-numbered columns have one extra hexagon
    /// compared to the odd-numbered ones.
    /// </summary>
    public interface IMap : IGameObject
    {
        void Initialize(int rows, int columns);

        int Rows { get; }

        int Columns { get; }

        /// <summary>
        /// Gets a tile based on the internal coordinate system of the map
        /// </summary>
        IHexTile GetTile(int row, int col);

        bool HasNeighbour(IHexTile tile, TileDirection direction);

        IHexTile GetNeighbour(IHexTile tile, TileDirection direction);
        
        List<IHexTile> GetNeighbours(IHexTile tile);

        /// <summary>
        /// Access a path by name
        /// </summary>
        /// <param name="pathName">The name of the path to get</param>
        /// <returns>An IPath instance</returns>
        MapPath GetPath(string pathName);

    }
}
