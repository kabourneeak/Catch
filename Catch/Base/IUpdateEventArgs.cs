namespace Catch.Base
{
    public interface IUpdateEventArgs
    {
        /// <summary>
        /// The elapsed ticks since this IUpdateable object was last called
        /// </summary>
        float Ticks { get; }

        ISimulationManager Manager { get; }

        ISimulationState Sim { get; }
    }
}