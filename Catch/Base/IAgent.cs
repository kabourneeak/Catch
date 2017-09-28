using System.Numerics;

namespace Catch.Base
{
    /// <summary>
    /// Basic details of an agent in the simulation
    /// </summary>
    /// <seealso cref="IExtendedAgent"/>
    public interface IAgent
    {
        string AgentType { get; }

        string DisplayName { get; }

        string DisplayInfo { get; }

        string DisplayStatus { get; }

        /// <summary>
        /// The world coordinates of the center of the agent
        /// </summary>
        Vector2 Position { get; }

        IMapTile Tile { get; }

        /// <summary>
        /// Regardless of how an agent makes its way through a tile, it spends 
        /// some amount of time in there. Considering the entrance and exit 
        /// times, the agent is some proportion of its way through the tile
        /// from 0.0 to 1.0.
        /// </summary>
        float TileProgress { get; }

        IVersionedEnumerable<IStatModifier<BaseStatsModel>> BaseModifiers { get; }

        IVersionedEnumerable<IStatModifier<AttackModel>> AttackModifiers { get; }

        IVersionedEnumerable<ILabel> Labels { get; }

        IVersionedEnumerable<IAgentCommand> Commands { get; }

        IBaseStats Stats { get; }
    }
}
