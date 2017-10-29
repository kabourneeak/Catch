using System;
using System.Numerics;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using CatchLibrary.Serialization;

namespace Catch.Level
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
        private readonly MapGraphicsProvider _mapGraphics;
        private readonly IAgentProvider _agentProvider;

        #region Construction

        public LevelController(IConfig config, MapSerializationModel mapSerializationModel)
        {
            /*
             * Bootstrap simulation
             */

            var mapProvider = new BuiltinMapProvider(config);

            var map = mapProvider.CreateMap(mapSerializationModel.Rows, mapSerializationModel.Columns);

            // TODO don't initialize this here
            _mapGraphics = new MapGraphicsProvider(config);

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
                tile.Indicators.Add(_mapGraphics.EmptyTileIndicator);

                var tileEmitModel = mapSerializationModel.Tiles.GetHex(tile.Coords);
                var towerArgs = new CreateAgentArgs()
                {
                    Tile = tile,
                    Team = tileEmitModel.Team
                };

                var tower = CreateAgent(tileEmitModel.TowerName, towerArgs);
                this.Register(tower);
                this.Site(tower);
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
            _mapGraphics.CreateResources(args);
            _overlayController.CreateResources(args);
        }

        public void DestroyResources()
        {
            _agentProvider.DestroyResources();
            _mapGraphics.DestroyResources();
            _overlayController.DestroyResources();
        }

        #endregion

        #region IDrawable Implementation

        public void Draw(DrawArgs drawArgs)
        {
            // the FieldController draws the field of play; the map, the agents, all the action
            _fieldController.Draw(drawArgs);

            // the overlay draws second so that it is on top
            _overlayController.Draw(drawArgs);
        }

        #endregion

        #region ISimulationManager Implementation

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

        #endregion
    }
}
