namespace Catch.Base
{
    /// <summary>
    /// Objects which have simulation state that is updated as the simulation
    /// proceeds
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Called during the game loop for updatable objects to advance their simulation
        /// e.g., move, target, attack, animate, etc.
        /// </summary>
        /// <param name="ticks">The elapsed simulation time since the last Update</param>
        void Update(float ticks);
    }
}
