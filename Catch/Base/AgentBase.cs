using System.Collections.Generic;
using System.Numerics;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Base
{
    /// <summary>
    /// Implements a basic IExtendedAgent, and is designed to be subclasses for the 
    /// Update behaviour.
    /// 
    /// Provides subclass sandbox methods for common actions that an agent may take,
    /// and delegates to modifiers as appropriate.
    /// </summary>
    public abstract class AgentBase : IExtendedAgent
    {
        private static readonly IComparer<IModifier> StatModelComparer =
            Comparer<IModifier>.Create((x, y) => x.Priority.CompareTo(y.Priority));

        private readonly IVersionedCollection<IModifier> _modifiers;
        private readonly IVersionedCollection<ILabel> _labels;
        private readonly IVersionedCollection<IAgentCommand> _commands;
        private readonly BaseStatsModel _stats;

        protected AgentBase(string agentType)
        {
            AgentType = agentType;
            Position = new Vector2(0.0f);

            Indicators = new IndicatorCollection();
            _modifiers = new VersionedCollection<IModifier>(new SimpleSortedList<IModifier>(StatModelComparer));
            _labels = new VersionedCollection<ILabel>(new HashSet<ILabel>());
            _commands = new VersionedCollection<IAgentCommand>(new HashSet<IAgentCommand>());

            _stats = new BaseStatsModel();
        }

        #region IAgent Properties

        public string AgentType { get; }
        public string DisplayName { get; protected set; }
        public string DisplayInfo { get; protected set; }
        public string DisplayStatus { get; protected set; }
        public Vector2 Position { get; set; }
        public IMapTile Tile { get; set; }
        public float TileProgress { get; set; }
        public IVersionedEnumerable<ILabel> Labels => _labels;
        public IVersionedEnumerable<IAgentCommand> Commands => _commands;
        public IBaseStats Stats => _stats;

        #endregion

        #region IExtendedAgent Properties

        public IVersionedCollection<ILabel> LabelCollection => _labels;
        public IVersionedCollection<IAgentCommand> CommandCollection => _commands;
        public IndicatorCollection Indicators { get; }
        public BaseStatsModel ExtendedStats => _stats;

        public void AddModifier(IModifier modifier)
        {
            _modifiers.Add(modifier);

            if (modifier is IAgentStatsModifier)
                CalculateAgentStats();
        }

        public void RemoveModifier(IModifier modifier)
        {
            var wasRemoved = _modifiers.Remove(modifier);

            if (!wasRemoved)
                return;

            if (modifier is IAgentStatsModifier)
                CalculateAgentStats();
        }

        #endregion

        #region IUpdatable Implementation

        public abstract float Update(IUpdateEventArgs args);

        #endregion

        #region IDrawable Implementation

        public virtual void Draw(DrawArgs drawArgs, float rotation)
        {
            if (Indicators.Count == 0)
                return;

            drawArgs.PushTranslation(Position);

            Indicators.Draw(drawArgs, rotation);

            drawArgs.Pop();
        }

        #endregion

        #region Events

        public void OnHit(AttackEventArgs e)
        {
            foreach (var modifier in _modifiers)
                if (modifier is IHitModifier hitModifier)
                    hitModifier.OnHit(this, e);
        }

        public void OnRemove()
        {
            foreach (var modifier in _modifiers)
                if (modifier is IRemoveModifier removeModifier)
                    removeModifier.OnRemove(this);
        }

        #endregion

        #region Subclass Sandbox

        /// <summary>
        /// Recalculates the agent stats (<see cref="IAgent.Stats"/> and <see cref="IAgent.Labels"/>. 
        /// 
        /// This is called automatically any time a modifier expressing the <see cref="IAgentStatsModifier"/> 
        /// interface is added or removed from the agent.
        /// </summary>
        protected void CalculateAgentStats()
        {
            foreach (var modifier in _modifiers)
                if (modifier is IAgentStatsModifier calculateAgentStatsModifier)
                    calculateAgentStatsModifier.OnCalculateAgentStats(this);
        }

        /// <summary>
        /// Creates an attack event and applies all registered <see cref="IAttackModifier"/>. The returned
        /// attack can then be applied to the target immediately or later (e.g., in the case of slow projectiles,
        /// the attack is created as soon as it is fired, but doesn't 'hit' until impact).
        /// </summary>
        /// <param name="target">The target of the attack</param>
        /// <returns>An instance of <see cref="AttackEventArgs"/> which can be passed to the target's OnHit event</returns>
        protected AttackEventArgs CreateAttack(IAgent target)
        {
            var attack = new AttackEventArgs(this, target);

            foreach (var modifier in _modifiers)
                if (modifier is IAttackModifier attackModifier)
                    attackModifier.OnAttack(this, attack);

            return attack;
        }

        #endregion
    }
}
