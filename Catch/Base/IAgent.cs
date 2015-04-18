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
    }
}
