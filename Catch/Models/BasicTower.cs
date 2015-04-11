using System;
using System.Numerics;
using Catch.Base;

namespace Catch.Models
{
    /// <summary>
    /// Implementation of ITower. Instantiates all of the underlying objects required for a "full" instance of
    /// an ITower, and exposes protected setters for the entire interface.
    /// 
    /// Dispatches update/create resources/draw to all child objects that require them in some sane order. Pushes
    /// and pops a translation to Draw.DrawArgs for the center point of the tower, so that all Indicators can 
    /// draw relatively.
    /// </summary>
    public abstract class BasicTower : ITower
    {
        protected BasicTower(IHexTile tile)
        {
            Tile = tile;
            Position = tile.Position;
            Indicators = new BasicIndicatorCollection();
        }

        #region IGameObject Implementation

        public string DisplayName { get; protected set; }
        public string DisplayInfo { get; protected set; }
        public string DisplayStatus { get; protected set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
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
