using System.Numerics;
using Catch.Base;

namespace Catch.Models
{
    public abstract class BasicMob : IMob
    {
        protected BasicMob()
        {
            Position = new Vector2(0.0f);
            Indicators = new BasicIndicatorCollection();
        }

        #region IGameObject Implementation

        public string DisplayName { get; protected set; }
        public string DisplayInfo { get; protected set; }
        public string DisplayStatus { get; protected set; }
        public Vector2 Position { get; set; }
        public DrawLayer Layer { get; protected set; }

        public void Update(float ticks)
        {
            Brain.Update(ticks);
            Indicators.Update(ticks);
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            Indicators.CreateResources(createArgs);
        }

        public void Draw(DrawArgs drawArgs)
        {
            if (Indicators.Count == 0)
                return;

            drawArgs.PushTranslation(Position);

            Indicators.Draw(drawArgs);

            drawArgs.Pop();
        }

        #endregion

        #region IAgent Implementation

        public abstract string GetAgentType();

        public IBehaviourComponent Brain { get; protected set; }
        public IHexTile Tile { get; set; }
        public bool IsTargetable { get; set; }
        public int Health { get; set; }
        public IModifierCollection Modifiers { get; protected set; }
        public IIndicatorCollection Indicators { get; protected set; }
        public AttackSpecs AttackSpecs { get; protected set; }
        public DefenceSpecs DefenceSpecs { get; protected set; }
        public IAgentStats Stats { get; protected set; }

        #endregion

    }
}
