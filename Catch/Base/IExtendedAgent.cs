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

        IVersionedCollection<IStatModifier<BaseStatsModel>> BaseModifierCollection { get; }

        IVersionedCollection<IStatModifier<AttackModel>> AttackModifierCollection { get; }

        IVersionedCollection<ILabel> LabelCollection { get; }

        IVersionedCollection<IAgentCommand> CommandCollection { get; }

        IndicatorCollection Indicators { get; }

        BaseStatsModel ExtendedStats { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the Agent is being removed from the simulation by the simulation manager
        /// </summary>
        void OnRemove();

        void OnChange(AttackModel attack);

        #endregion
    }
}
