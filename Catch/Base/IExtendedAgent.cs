using System;
using Catch.Graphics;

namespace Catch.Base
{
    /// <summary>
    /// Extended details of an agent in the simulation, as well as access to mutable properties
    /// and other methods for working with the agent.
    /// </summary>
    /// <seealso cref="IAgent"/>
    public interface IExtendedAgent : IUpdatable, IDrawable, IAgent
    {
        #region Properties

        IVersionedCollection<ILabel> LabelCollection { get; }

        IVersionedCollection<IAgentCommand> CommandCollection { get; }

        IndicatorCollection Indicators { get; }

        BaseStatsModel ExtendedStats { get; }

        #endregion

        #region Methods

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
