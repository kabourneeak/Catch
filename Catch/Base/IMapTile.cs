using System.Collections.Generic;
using System.Numerics;
using CatchLibrary.HexGrid;

namespace Catch.Base
{
    public interface IMapTile
    {
        HexCoords Coords { get; }

        Vector2 Position { get; }

        ITileAgent TileAgent { get; }

        int AgentCount { get; }

        IEnumerable<IAgent> Agents { get; }
    }
}
