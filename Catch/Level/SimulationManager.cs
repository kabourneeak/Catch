using System;
using Catch.Base;
using Catch.Map;

namespace Catch.Level
{
    public class SimulationManager : ISimulationManager
    {
        private readonly LevelStateModel _level;
        private readonly UpdateController _updateController;
        private readonly IAgentProvider _agentProvider;

        public SimulationManager(LevelStateModel level, UpdateController updateController, IAgentProvider agentProvider)
        {
            _level = level ?? throw new ArgumentNullException(nameof(level));
            _updateController = updateController ?? throw new ArgumentNullException(nameof(updateController));
            _agentProvider = agentProvider ?? throw new ArgumentNullException(nameof(agentProvider));
        }

        public void Register(IExtendedAgent agent)
        {
            if (agent == null)
                throw new ArgumentNullException(nameof(agent));

            this._updateController.Register(agent);

            // this is the only way to register an agent to a tile without getting unregister
            // called. And unregister checks that the agent was properly registered before
            // unregistering, which forces new agents to go through this method before they
            // can be moved.
            RegisterToTile(agent, agent.Tile);
        }

        public void Register(IUpdatable updatable)
        {
            if (updatable == null)
                throw new ArgumentNullException(nameof(updatable));

            this._updateController.Register(updatable);
        }

        public IExtendedAgent CreateAgent(string agentName, CreateAgentArgs createArgs)
        {
            return _agentProvider.CreateAgent(agentName, createArgs);
        }

        public void Remove(IExtendedAgent agent)
        {
            agent.OnRemove();

            UnregisterFromTile(agent);
        }

        public void Move(IExtendedAgent agent, IMapTile tile)
        {
            if (!ReferenceEquals(tile, agent.Tile))
            {
                UnregisterFromTile(agent);
                RegisterToTile(agent, tile);
            }
        }

        public void Site(IExtendedAgent agent)
        {
            if (agent == null)
                throw new ArgumentNullException(nameof(agent));

            if (object.ReferenceEquals(agent.Tile, _level.OffMap))
            {
                // for the off map tile, we don't actually site the agent
            }
            else
            {
                var tileModel = _level.Map.GetTileModel(agent.Tile);

                if (tileModel.TileAgent != null)
                    throw new ArgumentException("Attempted to site agent to tile which already has a TileAgent");

                tileModel.SetTileAgent(agent);
            }
        }

        private void RegisterToTile(IExtendedAgent agent, IMapTile tile)
        {
            if (agent == null)
                throw new ArgumentNullException(nameof(agent));
            if (tile == null)
                throw new ArgumentNullException(nameof(tile));

            if (object.ReferenceEquals(tile, _level.OffMap))
            {
                _level.OffMap.AddAgent(agent);

                // reset tile reference
                agent.Tile = _level.OffMap;
            }
            else
            {
                var tileModel = _level.Map.GetTileModel(tile);

                tileModel.AddAgent(agent);

                // reset tile reference
                agent.Tile = tileModel;
            }
        }

        private void UnregisterFromTile(IExtendedAgent agent)
        {
            if (agent.Tile == null)
                throw new ArgumentNullException(nameof(agent), "Agent tile was set to null");

            MapTileModel tileModel;

            if (ReferenceEquals(agent.Tile, _level.OffMap))
            {
                tileModel = _level.OffMap;
            }
            else
            {
                tileModel = _level.Map.GetTileModel(agent.Tile);

                // un-site the agent, if it was the tile agent
                if (ReferenceEquals(agent.Tile.TileAgent, agent))
                    tileModel.SetTileAgent(null);
            }

            var wasRemoved = tileModel.RemoveAgent(agent);

            if (!wasRemoved)
                throw new ArgumentException("Attempted to unregister agent from Tile which did not contain it", nameof(agent));
        }
    }
}
