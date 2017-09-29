using System;
using System.Numerics;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using CatchLibrary.Serialization;

namespace Catch
{
    /// <summary>
    /// Controls the execution of a level, executing instructions from a map definition
    /// </summary>
    public class LevelController : IScreenController, ISimulationManager
    {
        private readonly UpdateController _updateController;
        private readonly FieldController _fieldController;
        private readonly OverlayController _overlayController;
        private readonly Random _rng = new Random();

        private readonly LevelStateModel _level;
        private readonly SimulationStateModel _sim;
        private readonly IAgentProvider _agentProvider;

        #region Construction

        public LevelController(IConfig config, MapSerializationModel mapSerializationModel)
        {
            /*
             * Bootstrap simulation
             */

            var mapProvider = new BuiltinMapProvider(config);

            var map = mapProvider.CreateMap(mapSerializationModel.Rows, mapSerializationModel.Columns);

            var labelProvider = new LabelProvider();
            _level = new LevelStateModel(config, map);
            _sim = new SimulationStateModel(config, _level.Map, _level.OffMap);

            _updateController = new UpdateController(this, _sim, labelProvider);

            _agentProvider = new BuiltinAgentProvider(config, labelProvider);

            InitializeMap(mapSerializationModel, map);
            InitializeEmitScript(mapSerializationModel);

            _overlayController = new OverlayController(_level, this, _sim, labelProvider);
            _fieldController = new FieldController(_level);
        }

        private void InitializeMap(MapSerializationModel mapSerializationModel, MapModel map)
        {
            /*
             * Process tile models
             */
            foreach (var tile in map.TileModels)
            {
                var tileEmitModel = mapSerializationModel.Tiles.GetHex(tile.Coords);
                var towerArgs = new CreateAgentArgs()
                {
                    Tile = tile,
                    Team = tileEmitModel.Team
                };

                var tower = CreateTileAgent(tileEmitModel.TowerName, towerArgs);
                this.Register(tower);
            }

            /*
             * Process paths
             */
            foreach (var pathModel in mapSerializationModel.Paths)
            {
                var mapPath = new MapPathModel();
                mapPath.Name = pathModel.PathName;

                foreach (var pathStep in pathModel.PathSteps)
                {
                    mapPath.Add(map.GetTileModel(pathStep.Coords));
                }

                map.AddPath(mapPath);
            }
        }

        private void InitializeEmitScript(MapSerializationModel mapSerializationModel)
        {
            foreach (var emitScriptEntry in mapSerializationModel.EmitScript)
            {
                for (var i = 0; i < emitScriptEntry.Count; ++i)
                {
                    var agentArgs = new CreateAgentArgs()
                    {
                        Path = _level.Map.GetPath(emitScriptEntry.PathName),
                        Tile = _level.OffMap,
                        Team = emitScriptEntry.Team
                    };

                    var offset = emitScriptEntry.BeginTime + (i * emitScriptEntry.DelayTime);
                    var task = new SpawnAgentTask(offset, emitScriptEntry.AgentTypeName, agentArgs);

                    this.Register(task);
                }
            }
        }

        #endregion

        #region IScreenController Implementation

        public void Initialize(Vector2 size)
        {
            _level.Ui.WindowSize = size;

            _overlayController.Initialize();
            _fieldController.Initialize();
        }

        public bool AllowPredecessorUpdate() => false;

        public bool AllowPredecessorDraw() => false;

        public bool AllowPredecessorInput() => false;

        private float _elapsedDeviceTicks = 0.0f;

        public void Update(float deviceTicks)
        {
            _elapsedDeviceTicks += deviceTicks;

            _updateController.Update(deviceTicks);
            _fieldController.Update(deviceTicks);
            _overlayController.Update(deviceTicks);
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(PanByEventArgs eventArgs)
        {
            _fieldController.PanBy(eventArgs);
        }

        public void ZoomToPoint(ZoomToPointEventArgs eventArgs)
        {
            _fieldController.ZoomToPoint(eventArgs);
        }

        public void Resize(Vector2 size)
        {
            _level.Ui.WindowSize = size;

            _fieldController.Resize(size);
            _overlayController.Resize(size);
        }

        public void Hover(HoverEventArgs eventArgs)
        {
            _overlayController.Hover(eventArgs);

            if (!eventArgs.Handled)
                _fieldController.Hover(eventArgs);
        }

        public void Touch(TouchEventArgs eventArgs)
        {
            _overlayController.Touch(eventArgs);

            if (!eventArgs.Handled)
                _fieldController.Touch(eventArgs);
        }

        public void KeyPress(KeyPressEventArgs eventArgs)
        {
            _overlayController.KeyPress(eventArgs);

            if (!eventArgs.Handled)
                _fieldController.KeyPress(eventArgs);
        }

        #endregion

        #region IGraphicsResource Implementation

        public void CreateResources(CreateResourcesArgs args)
        {
            _agentProvider.CreateResources(args);
            _overlayController.CreateResources(args);
        }

        public void DestroyResources()
        {
            _agentProvider.DestroyResources();
            _overlayController.DestroyResources();
        }

        #endregion

        #region IDrawable Implementation

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            // the FieldController draws the field of play; the map, the agents, all the action
            _fieldController.Draw(drawArgs, rotation);

            // the overlay draws second so that it is on top
            _overlayController.Draw(drawArgs, rotation);
        }

        #endregion

        #region ISimulationManager Implementation

        public void Register(IExtendedAgent agent)
        {
            if (agent == null)
                throw new ArgumentNullException(nameof(agent));

            this._updateController.Register(agent);

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

        public IExtendedTileAgent CreateTileAgent(string agentName, CreateAgentArgs createArgs)
        {
            return (IExtendedTileAgent) _agentProvider.CreateAgent(agentName, createArgs);
        }

        public void Remove(IExtendedAgent agent)
        {
            agent.OnRemove();

            UnregisterFromTile(agent);
        }

        public IMapTile Move(IExtendedAgent agent, IMapTile tile)
        {
            UnregisterFromTile(agent);
            return RegisterToTile(agent, tile);
        }

        private IExtendedAgent PromoteAgent(IAgent agent)
        {
            return agent as IExtendedAgent;
        }

        private IMapTile RegisterToTile(IExtendedAgent agent, IMapTile tile)
        {
            if (agent == null)
                throw new ArgumentNullException(nameof(agent));
            if (tile == null)
                throw new ArgumentNullException(nameof(tile));

            if (object.ReferenceEquals(tile, _level.OffMap))
            {
                // for the off map tile, we don't care about whether the agent is a tile agent, 
                // nor whether we have more than one tile agent

                _level.OffMap.AddAgent(agent);

                return _level.OffMap;
            }
            else
            {
                var tileModel = _level.Map.GetTileModel(tile);

                tileModel.AddAgent(agent);

                if (agent is IExtendedTileAgent tileAgent)
                {
                    if (tileModel.TileAgent != null)
                        throw new ArgumentException("Attempted to register agent to tile which already has a TileAgent", nameof(tile));

                    tileModel.SetTileAgent(tileAgent);
                }

                return tileModel;
            }
        }

        private void UnregisterFromTile(IExtendedAgent agent)
        {
            if (agent.Tile == null)
                throw new ArgumentNullException(nameof(agent), "Agent tile was set to null");

            MapTileModel tileModel;

            if (object.ReferenceEquals(agent.Tile, _level.OffMap))
            {
                tileModel = _level.OffMap;
            }
            else
            {
                tileModel = _level.Map.GetTileModel(agent.Tile);

                if (agent is IExtendedTileAgent && object.ReferenceEquals(agent.Tile.TileAgent, agent))
                    tileModel.SetTileAgent(null);
            }

            var wasRemoved = tileModel.RemoveAgent(agent);

            if (!wasRemoved)
                throw new ArgumentException("Attempted to unregister agent from Tile which did not contain it", nameof(agent));
        }

        #endregion
    }
}
