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
        protected AgentBase()
        {
            Position = new Vector2(0.0f);
            Indicators = new IndicatorCollection();
            Modifiers = new ModifierCollection(this);
            IsActive = true;
            BaseSpecs = new BaseSpecModel();
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
        public DrawLayer Layer { get; protected set; }

        public virtual void Update(float ticks)
        {
            Modifiers.Update(ticks);
            Brain.Update(ticks);
            Indicators.Update(ticks);
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

        public bool IsActive { get; set; }
        public Tile Tile { get; set; }
        public bool IsTargetable { get; set; }
        public ModifierCollection Modifiers { get; protected set; }
        public IndicatorCollection Indicators { get; protected set; }
        public BaseSpecModel BaseSpecs { get; protected set; }
        public IAgentStats Stats { get; protected set; }

        public void PerformAttack(IAgent target)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnRemove()
        {
            Brain.OnRemove();
        }

        public virtual void OnHit(AttackModel incomingAttack)
        {
            Brain.OnHit(incomingAttack);
        }

        #endregion
    }
}
