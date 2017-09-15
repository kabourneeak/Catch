using System.Collections.Generic;
using System.Numerics;
using CatchLibrary.HexGrid;

namespace Catch.Base
{
    /// <summary>
    /// A container for everything that occupies the <see cref="Coords"/> of the simulation
    /// </summary>
    public interface IMapTile
    {
        /// <summary>
        /// The address of the tile
        /// </summary>
        HexCoords Coords { get; }

        /// <summary>
        /// The world coordinates of the center of the tile
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// The fixed agent occupying the tile; null if empty.
        /// </summary>
        ITileAgent TileAgent { get; }

        /// <summary>
        /// The number of agents currently occupying the tile, including the TileAgent
        /// </summary>
        int AgentCount { get; }

        /// <summary>
        /// All agents currently occupying the tile, including the TileAgent
        /// </summary>
        IEnumerable<IAgent> Agents { get; }

        /// <summary>
        /// The current version of the agent collection; incremented for every change to the <see cref="Agents"/> collection
        /// </summary>
        int AgentVersion { get; }

        /// <summary>
        /// The current version of the TileAgent property; incremented for every change to the <see cref="TileAgent"/> property
        /// </summary>
        int TileAgentVersion { get; }
    }
}
