using System.Collections.Generic;
using System.Numerics;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Base
{
    /// <summary>
    /// Instantiates all of the underlying objects required for a basic Agent.
    /// </summary>
    public abstract class AgentBase : IAgent
    {
        private static readonly IComparer<IStatModifier<StatModel>> StatModelComparer =
            Comparer<IStatModifier<StatModel>>.Create((x, y) => x.Priority.CompareTo(y.Priority));

        private static readonly IComparer<IStatModifier<AttackModel>> AttackModelComparer =
            Comparer<IStatModifier<AttackModel>>.Create((x, y) => x.Priority.CompareTo(y.Priority));

        protected AgentBase(string agentType)
        {
            AgentType = agentType;
            Position = new Vector2(0.0f);
            Indicators = new IndicatorCollection();
            BaseModifiers =
                new VersionedCollection<IStatModifier<StatModel>>(
                    new SimpleSortedList<IStatModifier<StatModel>>(StatModelComparer));
            AttackModifiers =
                new VersionedCollection<IStatModifier<AttackModel>>(
                    new SimpleSortedList<IStatModifier<AttackModel>>(AttackModelComparer));
            Labels = new VersionedCollection<ILabel>(new HashSet<ILabel>());
            Commands = new VersionedCollection<IAgentCommand>(new HashSet<IAgentCommand>());
            Stats = new StatModel();

            IsActive = true;
        }

        #region AgentBase Implementation

        protected IBehaviourComponent Brain { get; set; }

        #endregion

        #region IGameObject Implementation

        public string DisplayName { get; protected set; }
        public string DisplayInfo { get; protected set; }
        public string DisplayStatus { get; protected set; }
        public Vector2 Position { get; set; }

        public virtual void Draw(DrawArgs drawArgs, float rotation)
        {
            if (Indicators.Count == 0)
                return;

            drawArgs.PushTranslation(Position);

            Indicators.Draw(drawArgs, rotation);

            drawArgs.Pop();
        }

        #endregion

        #region IAgent Implementation

        public string AgentType { get; }
        public bool IsActive { get; set; }
        public IMapTile Tile { get; set; }
        public float TileProgress { get; set; }
        public IVersionedCollection<IStatModifier<StatModel>> BaseModifiers { get; }
        public IVersionedCollection<IStatModifier<AttackModel>> AttackModifiers { get; }
        public IVersionedCollection<ILabel> Labels { get; }
        public IndicatorCollection Indicators { get; }
        public IVersionedCollection<IAgentCommand> Commands { get; }
        public StatModel Stats { get; }

        public virtual float Update(IUpdateEventArgs args) => Brain.Update(args);

        public virtual void OnRemove()
        {
            Brain.OnRemove();

            IsActive = false;
        }

        public virtual void OnHit(AttackModel incomingAttack)
        {
            Brain.OnHit(incomingAttack);
        }

        #endregion
    }
}
