﻿namespace Catch.Base
{
    public interface IAgent : IGameObject
    {
        // Identification
        string GetAgentType();
        
        // Components
        IBehaviourComponent Brain { get; }

        // Properties
        Tile Tile { get; set; }

        bool IsTargetable { get; set; }

        int Health { get; set; }

        ModifierCollection Modifiers { get; }

        IndicatorCollection Indicators { get; }

        AttackSpecs AttackSpecs { get; }

        DefenceSpecs DefenceSpecs { get; }

        IAgentStats Stats { get; }
    }
}
