namespace Catch.Base
{
    /// <summary>
    /// The main control plane by which objects in the simulation can modify the simulation itself
    /// </summary>
    public interface ISimulationManager
    {
        void Register(IAgent agent);

        void Register(IUpdatable updatable);

        IAgent CreateAgent(string agentName, CreateAgentArgs createArgs);

        ITileAgent CreateTileAgent(string agentName, CreateAgentArgs createArgs);

        void Remove(IAgent agent);
    }
}
