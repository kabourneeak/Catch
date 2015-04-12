﻿using System.Numerics;
using Catch.Models;

namespace Catch.Base
{
    public abstract class Mob : IAgent
    {
        protected Mob()
        {
            Position = new Vector2(0.0f);
            Indicators = new IndicatorCollection();
        }

        #region Mob Specific

        /// <summary>
        /// Regardless of how a mob makes its way through a tile, it spends 
        /// some amount of time in there. Considering the entrance and exit 
        /// times, the mob is some proportion of its way through the tile
        /// from 0.0 to 1.0.
        /// </summary>
        public float TileProgress { get; set; }

        #endregion

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
