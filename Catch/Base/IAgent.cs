using System.Numerics;
using Catch.Graphics;
using Catch.Map;

namespace Catch.Base
{
    public interface IAgent : IUpdatable, IDrawable
    {
        #region Properties

        string AgentType { get; }

        string DisplayName { get; }

        string DisplayInfo { get; }

        string DisplayStatus { get; }

        Vector2 Position { get; set; }

        /// <summary>
        /// Indicates whether the GameObject is participating in the game.  
        /// Once set to false, the object will be removed from the game on
        /// the next update cycle.
        /// </summary>
        bool IsActive { get; }

        Tile Tile { get; }

        ModifierCollection Modifiers { get; }

        IndicatorCollection Indicators { get; }

        StatModel Stats { get; }

        CommandCollection Commands { get; }

        #endregion

        #region Events

        /// <summary>
        /// Called when the Agent is being removed from the game due to some external reason
        /// (e.g., a tower is sold). Agents must set IsActive to false when this is called.
        /// 
        /// When an agent sets IsActive to false of its own accord, this method is not called.
        /// </summary>
        void OnRemove();

        void OnHit(AttackModel incomingAttack);

        #endregion
    }
}
