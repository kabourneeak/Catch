﻿using System;
using Catch.Base;
using Catch.Map;

namespace Catch.Level
{
    public class SimulationManager : ISimulationManager
    {
        private readonly UpdateController _updateController;
        private readonly MapModel _map;
        private readonly IAgentProvider _agentProvider;
        private readonly IIndicatorRegistry _indicatorRegistry;

        public SimulationManager(UpdateController updateController, MapModel map, IAgentProvider agentProvider, IIndicatorRegistry indicatorRegistry)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _updateController = updateController ?? throw new ArgumentNullException(nameof(updateController));
            _agentProvider = agentProvider ?? throw new ArgumentNullException(nameof(agentProvider));
            _indicatorRegistry = indicatorRegistry ?? throw new ArgumentNullException(nameof(indicatorRegistry));
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

            _indicatorRegistry.Unregister(agent.Indicators);
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

            if (object.ReferenceEquals(agent.Tile, _map.OffMapTile))
            {
                // for the off map tile, we don't actually site the agent
            }
            else
            {
                var tileModel = _map.GetTileModel(agent.Tile);

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

            if (object.ReferenceEquals(tile, _map.OffMapTile))
            {
                _map.OffMapTileModel.AddAgent(agent);

                // reset tile reference
                agent.Tile = _map.OffMapTileModel;
            }
            else
            {
                var tileModel = _map.GetTileModel(tile);

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

            if (ReferenceEquals(agent.Tile, _map.OffMapTileModel))
            {
                tileModel = _map.OffMapTileModel;
            }
            else
            {
                tileModel = _map.GetTileModel(agent.Tile);

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
