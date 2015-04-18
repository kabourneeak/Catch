namespace Catch.Base
{
    public interface IAgent : IGameObject
    {
        // Identification
        string GetAgentType();
        
        // Components
        IBehaviourComponent Brain { get; }

        #region Properties

        /// <summary>
        /// Indicates whether the GameObject is participating in the game.  
        /// Once set to false, the object will be removed from the game on
        /// the next update cycle.
        /// </summary>
        bool IsActive { get; set; }

        Tile Tile { get; set; }

        bool IsTargetable { get; set; }

        int Health { get; set; }

        ModifierCollection Modifiers { get; }

        IndicatorCollection Indicators { get; }

        AttackSpecs AttackSpecs { get; }

        DefenceSpecs DefenceSpecs { get; }

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

        void OnAttacked(IAttack attack);

        #endregion
    }
}
