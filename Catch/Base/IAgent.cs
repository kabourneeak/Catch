using System;
using Catch.Graphics;

namespace Catch.Base
{
    /// <summary>
    /// Basic details of an agent in the simulation
    /// </summary>
    /// <seealso cref="IExtendedAgent"/>
    public interface IAgent : IDrawable
    {
        /// <summary>
        /// The agent type, set at construction
        /// </summary>
        string AgentType { get; }

        /// <summary>
        /// The map tile the agent is registered to
        /// </summary>
        IMapTile Tile { get; }

        /// <summary>
        /// Regardless of how an agent makes its way through a tile, it spends 
        /// some amount of time in there. Considering the entrance and exit 
        /// times, the agent is some proportion of its way through the tile
        /// from 0.0 to 1.0.
        /// </summary>
        float TileProgress { get; }

        IVersionedEnumerable<ILabel> Labels { get; }

        IVersionedEnumerable<IAgentCommand> Commands { get; }

        IBaseStats Stats { get; }

        #region Events

        /// <summary>
        /// Called when the agent is attacked by another
        /// </summary>
        /// <param name="e">Attack parameters</param>
        void OnHit(AttackEventArgs e);

        #endregion
    }
}
