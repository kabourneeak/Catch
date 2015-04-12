﻿using System.Numerics;
using Catch.Models;

namespace Catch.Base
{
    /// <summary>
    /// Instantiates all of the underlying objects required for a "full" instance of a Tower
    /// 
    /// Dispatches update/create resources/draw to all child objects that require them in some sane order. Pushes
    /// and pops a translation to Draw.DrawArgs for the center point of the tower, so that all Indicators can 
    /// draw relatively.
    /// </summary>
    public abstract class Tower : IAgent
    {
        protected Tower(Tile tile)
        {
            Tile = tile;
            Position = tile.Position;
            Layer = DrawLayer.Tower;
            Indicators = new IndicatorCollection();

            // site into tile
            tile.SetTower(this);
        }

        #region IGameObject Implementation

        public string DisplayName { get; protected set; }
        public string DisplayInfo { get; protected set; }
        public string DisplayStatus { get; protected set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public DrawLayer Layer { get; protected set; }

        public virtual void Update(float ticks)
        {
            Brain.Update(ticks);
            Indicators.Update(ticks);
        }

        public virtual void CreateResources(CreateResourcesArgs createArgs)
        {
            Indicators.CreateResources(createArgs);
        }

        public virtual void Draw(DrawArgs drawArgs)
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
        public Tile Tile { get; set; }
        public bool IsTargetable { get; set; }
        public int Health { get; set; }
        public ModifierCollection Modifiers { get; protected set; }
        public IndicatorCollection Indicators { get; protected set; }
        public AttackSpecs AttackSpecs { get; protected set; }
        public DefenceSpecs DefenceSpecs { get; protected set; }
        public IAgentStats Stats { get; protected set; }

        #endregion

    }
}
