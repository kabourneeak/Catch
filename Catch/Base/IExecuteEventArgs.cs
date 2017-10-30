namespace Catch.Base
{
    /// <summary>
    /// The simulation control plane provided to IAgentCommand objects when executing
    /// </summary>
    public interface IExecuteEventArgs
    {
        ISimulationState Sim { get; }

        ISimulationManager Manager { get; }

        IAgent SelectedAgent { get; }

        IAgent SelectedTileAgent { get; }
    }
}