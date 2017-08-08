using System.Collections.Generic;
using System.Numerics;
using CatchLibrary.HexGrid;

namespace Catch.Base
{
    /// <summary>
    /// A map, as visible from inside the simulation.
    /// </summary>
    public interface IMap
    {
        int Rows { get; }

        int Columns { get; }

        Vector2 Size { get; }

        IEnumerable<IMapTile> Tiles { get; }

        bool HasHex(HexCoords hc);

        IMapTile GetTile(HexCoords hc);

        IMapTile GetNeighbour(IMapTile tile, HexDirection direction);

        IEnumerable<IMapTile> GetNeighbours(IMapTile tile, int radius);

        /// <summary>
        /// Get all neighbouring tiles to the given tile within the band defined by fromRadius and toRadius, inclusive.
        /// </summary>
        IEnumerable<IMapTile> GetNeighbours(IMapTile tile, int fromRadius, int toRadius);

        IEnumerable<IMapPath> Paths { get; }

        IMapPath GetPath(string pathName);
    }
}
