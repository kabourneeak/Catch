namespace Catch.Base
{
    /// <summary>
    /// The main control plane by which objects in the simulation can modify the simulation itself
    /// </summary>
    public interface ISimulationManager
    {
        /// <summary>
        /// Register an agent to the simulation; 
        /// 
        /// The agent will be sited onto the tile that it has set on its Tile property, and registered
        /// as the TileAgent, if appropriate.
        /// 
        /// The agent will be scheduled for an initial Update in short order.
        /// </summary>
        /// <param name="agent">The agent to register</param>
        void Register(IExtendedAgent agent);

        /// <summary>
        /// Register a task to the simulation. It will be scheduled for an initial Update in short order
        /// </summary>
        /// <param name="updatable"></param>
        void Register(IUpdatable updatable);

        /// <summary>
        /// Create a new, unregistered agent.
        /// </summary>
        /// <param name="agentName">The agent name to be created</param>
        /// <param name="createArgs">Creation parameters to pass along to the agent provider</param>
        /// <returns>The newly created agent</returns>
        IExtendedAgent CreateAgent(string agentName, CreateAgentArgs createArgs);

        /// <summary>
        /// Create a new, unregistered tile agent.
        /// </summary>
        /// <param name="agentName">The agent name to be created</param>
        /// <param name="createArgs">Creation parameters to pass along to the agent provider</param>
        /// <returns>The newly created tile agent</returns>
        IExtendedTileAgent CreateTileAgent(string agentName, CreateAgentArgs createArgs);

        /// <summary>
        /// Remove the given agent from the map. Note that this will NOT prevent the agent from having updates
        /// scheduled. The <see cref="IExtendedAgent.OnRemove"/> method will be called just before the removal 
        /// takes place. An agent may call this on itself.
        /// </summary>
        /// <param name="agent">The agent to be removed</param>
        void Remove(IExtendedAgent agent);

        /// <summary>
        /// Move an agent from one Tile to another, or to the <see cref="ISimulationState.OffMap"/> tile. 
        /// When complete, the agent should set its Tile property equal to the return value.
        /// </summary>
        /// <param name="agent">The agent to move</param>
        /// <param name="tile">The new location</param>
        /// <returns>The newly updated tile which the agent is now registered to</returns>
        IMapTile Move(IExtendedAgent agent, IMapTile tile);
    }
}
