namespace Catch.Base
{
    public enum ModifierPriority
    {
        /// <summary>
        /// Modifier which sets the basic specs which define the Agent, such as MaxHealth, Level, and so on
        /// </summary>
        Base, 
        
        /// <summary>
        /// Modifier from the Game Setting level, such as overall difficulty
        /// </summary>
        GameSettings, 
        
        /// <summary>
        /// Modifier coming from a player-specific ability
        /// </summary>
        Player, 
        
        /// <summary>
        /// Modifier that generally improves the Agent
        /// </summary>
        Buff,

        /// <summary>
        /// Modifier that generally harms the Agent
        /// </summary>
        Debuff
    }
}