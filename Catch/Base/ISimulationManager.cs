namespace Catch.Base
{
    /// <summary>
    /// The main control plane by which objects in the simulation can modify the simulation itself
    /// </summary>
    public interface ISimulationManager
    {
        void Register(IExtendedAgent agent);

        void Register(IUpdatable updatable);

        IExtendedAgent CreateAgent(string agentName, CreateAgentArgs createArgs);

        IExtendedTileAgent CreateTileAgent(string agentName, CreateAgentArgs createArgs);

        void Remove(IExtendedAgent agent);

        IMapTile Move(IExtendedAgent agent, IMapTile tile);
    }
}
