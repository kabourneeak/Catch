using System.Numerics;
using Catch.Graphics;

namespace Catch.Base
{
    /// <summary>
    /// Extended details of an agent in the simulation, as well as access to mutable properties
    /// and other methods for working with the agent.
    /// </summary>
    /// <seealso cref="IAgent"/>
    public interface IExtendedAgent : IUpdatable, IAgent
    {
        #region Properties

        new Vector2 Position { set; get; }

        new IMapTile Tile { set; get; }

        new float TileProgress { get; set; }

        IVersionedCollection<ILabel> LabelCollection { get; }

        IVersionedCollection<IAgentCommand> CommandCollection { get; }

        BaseStatsModel ExtendedStats { get; }

        #endregion

        #region Methods

        void Draw(DrawArgs drawArgs);

        void AddModifier(IModifier modifier);

        void RemoveModifier(IModifier modifier);

        #endregion

        #region Events

        /// <summary>
        /// Called when the Agent is being removed from the simulation by the simulation manager
        /// </summary>
        void OnRemove();

        #endregion
    }
}
