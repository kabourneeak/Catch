namespace Catch.Base
{
    /// <summary>
    /// Objects which have simulation state that is updated as the simulation
    /// proceeds
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Called when it is the IUpdatable object's turn to update
        /// </summary>
        /// <param name="args">The elapsed simulation time since the last Update</param>
        /// <returns>The number of ticks the IUpdatable would like to be called again. Use zero or less to be deregistered</returns>
        float Update(IUpdateEventArgs args);
    }
}
