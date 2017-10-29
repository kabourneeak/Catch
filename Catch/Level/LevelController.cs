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
    public class LevelController : IScreenController
    {
        private readonly UpdateController _updateController;
        private readonly FieldController _fieldController;
        private readonly OverlayController _overlayController;
        private readonly Random _rng = new Random();

        private readonly LevelStateModel _level;
        private readonly SimulationStateModel _sim;
        private readonly MapGraphicsProvider _mapGraphics;
        private readonly IAgentProvider _agentProvider;
        private readonly IGraphicsManager _graphicsManager;
        private readonly ISimulationManager _simulationManager;
        private readonly UpdateEventArgs _updateEventArgs;

        private float _elapsedDeviceTicks = 0.0f;

        #region Construction

        public LevelController(IConfig config, MapSerializationModel mapSerializationModel)
        {
            /*
             * Bootstrap simulation
             */

            var mapProvider = new BuiltinMapProvider(config);

            var map = mapProvider.CreateMap(mapSerializationModel.Rows, mapSerializationModel.Columns);

            _graphicsManager = new GraphicsManager(config);

            _mapGraphics = _graphicsManager.Resolve<MapGraphicsProvider>();

            var labelProvider = new LabelProvider();
            _level = new LevelStateModel(config, map);
            _sim = new SimulationStateModel(config, _level.Map, _level.OffMap);

            _agentProvider = new BuiltinAgentProvider(config, labelProvider);

            _updateController = new UpdateController();

            _simulationManager = new SimulationManager(_level, _updateController, _agentProvider);

            _updateEventArgs = new UpdateEventArgs(_simulationManager, _sim, labelProvider);

            InitializeMap(mapSerializationModel, map);
            InitializeEmitScript(mapSerializationModel);

            _overlayController = new OverlayController(_level, _simulationManager, _sim, labelProvider, _graphicsManager);
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

                var tower = _simulationManager.CreateAgent(tileEmitModel.TowerName, towerArgs);
                _simulationManager.Register(tower);
                _simulationManager.Site(tower);
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

                    _simulationManager.Register(task);
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

        public void Update(float deviceTicks)
        {
            _elapsedDeviceTicks += deviceTicks;

            _updateController.Update(deviceTicks, _updateEventArgs);
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
            _graphicsManager.CreateResources(args);
            _agentProvider.CreateResources(args);
        }

        public void DestroyResources()
        {
            _graphicsManager.DestroyResources();
            _agentProvider.DestroyResources();
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

    }
}
