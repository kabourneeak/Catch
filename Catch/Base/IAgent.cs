using System;
using System.Numerics;
using Catch.Graphics;

namespace Catch.Base
{
    public interface IAgent : IUpdatable, IDrawable
    {
        #region Properties

        string AgentType { get; }

        string DisplayName { get; }

        string DisplayInfo { get; }

        string DisplayStatus { get; }

        /// <summary>
        /// The world coordinates of the center of the agent
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Indicates whether the GameObject is participating in the game.  
        /// Once set to false, the object will be removed from the game on
        /// the next update cycle.
        /// </summary>
        [Obsolete]
        bool IsActive { get; }

        IMapTile Tile { get; }

        /// <summary>
        /// Regardless of how an agent makes its way through a tile, it spends 
        /// some amount of time in there. Considering the entrance and exit 
        /// times, the agent is some proportion of its way through the tile
        /// from 0.0 to 1.0.
        /// </summary>
        float TileProgress { get; }

        ModifierCollection Modifiers { get; }

        IndicatorCollection Indicators { get; }

        StatModel Stats { get; }

        CommandCollection Commands { get; }

        #endregion

        #region Events

        /// <summary>
        /// Called when the Agent is being removed from the simulation by the simulation manager
        /// </summary>
        void OnRemove();

        void OnHit(AttackModel incomingAttack);

        #endregion
    }
}
