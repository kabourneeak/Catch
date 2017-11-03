﻿using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Components;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using CatchLibrary.Serialization.Maps;
using Unity;

namespace Catch.Level
{
    /// <summary>
    /// Controls the execution of a level, executing instructions from a map definition
    /// </summary>
    public class LevelController : IScreenController
    {
        private readonly IUnityContainer _levelContainer;
        private readonly UpdateController _updateController;
        private readonly FieldController _fieldController;
        private readonly OverlayController _overlayController;

        private readonly GraphicsResourceManager _graphicsResourceManager;
        private readonly ISimulationManager _simulationManager;
        private readonly UpdateEventArgs _updateEventArgs;
        private readonly MapModel _map;
        private readonly UiStateModel _uiState;

        #region Construction

        public LevelController(IConfig config, MapSerializationModel mapSerializationModel)
        {
            /*
             * Bootstrap simulation
             */
            _levelContainer = LevelBootstrapper.CreateContainer(config);

            _graphicsResourceManager = _levelContainer.Resolve<GraphicsResourceManager>();

            _map = _levelContainer.Resolve<MapModel>();
            _uiState = _levelContainer.Resolve<UiStateModel>();

            _updateController = _levelContainer.Resolve<UpdateController>();
            _simulationManager = _levelContainer.Resolve<ISimulationManager>();

            _overlayController = _levelContainer.Resolve<OverlayController>();
            _fieldController = _levelContainer.Resolve<FieldController>();

            /*
             * Other initialization
             */
            InitializeMap(mapSerializationModel, _map);
            InitializeEmitScript(mapSerializationModel);

            _updateEventArgs = new UpdateEventArgs(_simulationManager, _levelContainer.Resolve<SimulationStateModel>());
        }

        private void InitializeMap(MapSerializationModel mapSerializationModel, MapModel map)
        {
            var indicatorProvider = _levelContainer.Resolve<IIndicatorProvider>();

            map.Initialize(mapSerializationModel.Rows, mapSerializationModel.Columns);

            /*
             * Process tile models
             */
            foreach (var tile in map.TileModels)
            {
                var ind = (SpriteIndicator) indicatorProvider.GetIndicator("EmptyTileIndicator", tile);
                ind.Position = tile.Position;
                tile.Indicators.Add(ind);

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

            ISet<MapTileModel> allPathTiles = new HashSet<MapTileModel>();
            foreach (var pathModel in mapSerializationModel.Paths)
            {
                var mapPath = new MapPathModel();
                mapPath.Name = pathModel.PathName;

                foreach (var pathStep in pathModel.PathSteps)
                {
                    var mapTileModel = map.GetTileModel(pathStep.Coords);
                    allPathTiles.Add(mapTileModel);
                    mapPath.Add(mapTileModel);
                }

                map.AddPath(mapPath);
            }

            foreach (var tile in allPathTiles)
            {
                var ind = (SpriteIndicator) indicatorProvider.GetIndicator("PathTileIndicator", tile);
                ind.Position = tile.Position;
                tile.Indicators.Add(ind);
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
                        Path = _map.GetPath(emitScriptEntry.PathName),
                        Tile = _map.OffMapTileModel,
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
            _uiState.WindowSize = size;

            _overlayController.Initialize();
            _fieldController.Initialize();
        }

        public bool AllowPredecessorUpdate() => false;

        public bool AllowPredecessorDraw() => false;

        public bool AllowPredecessorInput() => false;

        public void Update(float deviceTicks)
        {
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
            _uiState.WindowSize = size;

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
            _graphicsResourceManager.CreateResources(args);
        }

        public void DestroyResources()
        {
            _graphicsResourceManager.DestroyResources();
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
