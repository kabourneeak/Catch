using Catch.Map;

namespace Catch.Base
{
    public interface IAgent : IGameObject
    {
        // Identification
        string GetAgentType();
        
        #region Properties

        /// <summary>
        /// Indicates whether the GameObject is participating in the game.  
        /// Once set to false, the object will be removed from the game on
        /// the next update cycle.
        /// </summary>
        bool IsActive { get; }

        ILevelStateModel Level { get; }

        Tile Tile { get; }

        bool IsTargetable { get; }

        ModifierCollection Modifiers { get; }

        IndicatorCollection Indicators { get; }

        BaseSpecModel BaseSpecs { get; }

        CommandCollection Commands { get; }

        IAgentStats Stats { get; }

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
