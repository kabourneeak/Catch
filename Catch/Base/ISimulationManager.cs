namespace Catch.Base
{
    /// <summary>
    /// The main control plane by which objects in the simulation can modify the simulation itself
    /// </summary>
    public interface ISimulationManager
    {
        /// <summary>
        /// Register an agent to the simulation; the agent will be registered with the tile that
        /// that it has set on its Tile property, but will not be set as the tile agent.
        /// 
        /// The agent will be scheduled for an initial Update in short order.
        /// </summary>
        /// <param name="agent">The agent to register</param>
        void Register(IExtendedAgent agent);

        /// <summary>
        /// Register a task to the simulation. 
        /// 
        /// The updatable will be scheduled for an initial Update in short order
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
        /// Remove the given agent from the map and the simulation. 
        /// 
        /// Note that this will NOT prevent the agent from having updates scheduled. The <see cref="IExtendedAgent.OnRemove"/> 
        /// method will be called just before the removal takes place. An agent may call this on itself. 
        /// 
        /// A removed agent can not be added back to the simulation; to temporarily remove an agent from the field of play,
        /// move it to the OffMap tile.
        /// </summary>
        /// <param name="agent">The agent to be removed</param>
        void Remove(IExtendedAgent agent);

        /// <summary>
        /// Move an agent from one Tile to another, or to the <see cref="ISimulationState.OffMap"/> tile. 
        /// When complete, the agent should set its Tile property equal to the return value.
        /// </summary>
        /// <param name="agent">The agent to move</param>
        /// <param name="tile">The new location</param>
        void Move(IExtendedAgent agent, IMapTile tile);

        /// <summary>
        /// Set the agent as the TileAgent on its current tile
        /// </summary>
        /// <param name="agent">The agent to site</param>
        void Site(IExtendedAgent agent);
    }
}
