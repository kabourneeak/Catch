using System.Numerics;
using Catch.Graphics;
using Catch.Map;

namespace Catch.Base
{
    /// <summary>
    /// Instantiates all of the underlying objects required for a basic Agent.
    /// </summary>
    public abstract class AgentBase : IAgent
    {
        protected AgentBase(ILevelStateModel level)
        {
            Level = level;
            Position = new Vector2(0.0f);
            Indicators = new IndicatorCollection();
            Modifiers = new ModifierCollection(this);
            Commands = new CommandCollection();
            BaseSpecs = new BaseSpecModel();
            Stats = new AgentStatsModel();

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
        public float Rotation { get; set; }

        public virtual void Update(float ticks)
        {
            Indicators.Update(ticks);
            Modifiers.Update(ticks);
            Brain.Update(ticks);
            Commands.Update(ticks);
        }

        public virtual void CreateResources(CreateResourcesArgs createArgs)
        {
            Indicators.CreateResources(createArgs);
        }

        public virtual void DestroyResources()
        {
            Indicators.DestroyResources();
        }

        public virtual void Draw(DrawArgs drawArgs, float rotation)
        {
            if (Indicators.Count == 0)
                return;

            drawArgs.PushTranslation(Position);

            // ignore the rotation parameter, and replace by our Rotation
            Indicators.Draw(drawArgs, Rotation);

            drawArgs.Pop();
        }

        #endregion

        #region IAgent Implementation

        public abstract string GetAgentType();
        public ILevelStateModel Level { get; }

        public bool IsActive { get; set; }
        public Tile Tile { get; set; }
        public bool IsTargetable { get; set; }
        public ModifierCollection Modifiers { get; }
        public IndicatorCollection Indicators { get; }
        public CommandCollection Commands { get; }
        public BaseSpecModel BaseSpecs { get; }
        public IAgentStats Stats { get; }

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
