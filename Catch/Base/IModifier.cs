namespace Catch.Base
{
    /// <summary>
    /// Defines an agent modifier, which is a sub-Task of sorts that will be called 
    /// by the agent whenever it is carrying out a modifiable action.
    /// 
    /// See derived interfaces
    /// </summary>
    public interface IModifier
    {
        /// <summary>
        /// The priority for this mod, which determines the order in which mods are called
        /// </summary>
        ModifierPriority Priority { get; }
    }
}