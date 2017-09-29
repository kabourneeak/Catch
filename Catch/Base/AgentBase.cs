using System.Collections.Generic;
using System.Numerics;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Base
{
    /// <summary>
    /// Instantiates all of the underlying objects required for a basic Agent.
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

        #region AgentBase Implementation

        protected IBehaviourComponent Brain { get; set; }

        #endregion

        #region IAgent Implementation

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

        #region IExtendedAgent Implementation

        public IVersionedCollection<IModifier> ModifierCollection => _modifiers;
        public IVersionedCollection<ILabel> LabelCollection => _labels;
        public IVersionedCollection<IAgentCommand> CommandCollection => _commands;
        public IndicatorCollection Indicators { get; }
        public BaseStatsModel ExtendedStats => _stats;

        public void OnCalculateAgentStats()
        {
            foreach (var modifier in ModifierCollection)
                if (modifier is ICalculateAgentStatsModifier calculateAgentStatsModifier)
                    calculateAgentStatsModifier.OnCalculateAgentStats(this);
        }

        public void OnAttack(AttackEventArgs e)
        {
            foreach (var modifier in ModifierCollection)
                if (modifier is IAttackModifier attackModifier)
                    attackModifier.OnAttack(this, e);
        }

        public void OnHit(AttackEventArgs e)
        {
            foreach (var modifier in ModifierCollection)
                if (modifier is IHitModifier hitModifier)
                    hitModifier.OnHit(this, e);
        }

        public void OnRemove()
        {
            foreach (var modifier in ModifierCollection)
                if (modifier is IRemoveModifier removeModifier)
                    removeModifier.OnRemove(this);
        }

        #endregion

        #region IUpdatable Implementation

        public virtual float Update(IUpdateEventArgs args) => Brain.Update(args);

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
    }
}
