﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.System;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using Catch.Win2d;
using CatchLibrary.Serialization;

namespace Catch
{
    /// <summary>
    /// Controls the execution of a level, executing instructions from a map definition
    /// </summary>
    public class LevelController : IGameController
    {
        private FieldController _fieldController;
        private OverlayController _overlayController;
        private readonly Random _rng = new Random();

        private readonly IConfig _config;
        private readonly UiStateModel _ui;
        private readonly PlayerModel _player;
        private readonly Win2DProvider _provider;
        private readonly List<IAgent> _agents;
        private readonly Queue<ScriptCommand> _scriptCommands;
        private Map.Map _map;

        #region Construction

        public LevelController(IConfig config)
        {
            _config = config;

            _ui = new UiStateModel();
            _player = new PlayerModel(_config);
            _provider = new Win2DProvider(_config);
            _agents = new List<IAgent>();
            _scriptCommands = new Queue<ScriptCommand>();
        }

        #endregion

        #region IGameController Implementation

        public event GameStateChangedHandler GameStateChangeRequested;

        public void Initialize(Vector2 size, GameStateArgs args)
        {
            _ui.WindowSize = size;

            _agents.Clear();

            InitializeMap(args.MapModel);

            _overlayController = new OverlayController(_ui, _agents, _map);
            _overlayController.Initialize();

            _fieldController = new FieldController(_ui, _agents, _map);
            _fieldController.Initialize();
        }

        private void InitializeMap(MapModel mapModel)
        {
            _map = _provider.CreateMap(mapModel.Rows, mapModel.Columns);

            /*
             * Process tile models
             */
            foreach (var tile in _map)
            {
                var tileModel = mapModel.Tiles.GetHex(tile.Coords);
                var tower = _provider.CreateTower(tileModel.TowerName, tile);
                _agents.Add(tower);
            }

            /*
             * Process paths
             */
            foreach (var pathModel in mapModel.Paths)
            {
                var mapPath = new MapPath();
                mapPath.Name = pathModel.PathName;

                foreach (var pathStep in pathModel.PathSteps)
                {
                    mapPath.Add(_map.GetHex(pathStep.Coords));
                }

                _map.AddPath(mapPath);
            }

            /*
             * Process emit script
             */
            var scriptCommandList = new List<ScriptCommand>();

            foreach (var emitScriptEntry in mapModel.EmitScript)
            {
                for (var i = 0; i < emitScriptEntry.Count; ++i)
                {
                    var emitCommand = new ScriptCommand
                    {
                        AgentTypeName = emitScriptEntry.AgentTypeName,
                        PathName = emitScriptEntry.PathName,
                        Offset = emitScriptEntry.BeginTime + (i * emitScriptEntry.DelayTime)
                    };

                    scriptCommandList.Add(emitCommand);
                }
            }

            scriptCommandList.Sort((x, y) => x.Offset.CompareTo(y.Offset));

            _scriptCommands.Clear();
            scriptCommandList.ForEach(c => _scriptCommands.Enqueue(c));
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(Vector2 panDelta)
        {
            _fieldController.PanBy(panDelta);
        }

        public void ZoomToPoint(Vector2 viewCoords, float zoomDelta)
        {
            _fieldController.ZoomToPoint(viewCoords, zoomDelta);
        }

        public void Resize(Vector2 size)
        {
            _ui.WindowSize = size;

            _fieldController.Resize(size);
            _overlayController.Resize(size);
        }

        public void Hover(Vector2 viewCoords, VirtualKeyModifiers keyModifiers)
        {
            _fieldController.Hover(viewCoords, keyModifiers);
            _overlayController.Hover(viewCoords, keyModifiers);
        }

        public void Touch(Vector2 viewCoords, VirtualKeyModifiers keyModifiers)
        {
            _fieldController.Touch(viewCoords, keyModifiers);
            _overlayController.Touch(viewCoords, keyModifiers);
        }

        public void KeyPress(VirtualKey key)
        {
            _fieldController.KeyPress(key);
            _overlayController.KeyPress(key);
        }

        #endregion

        #region IGraphicsComponent Implementation

        private float _elapsedTicks = 0.0f;

        public void Update(float ticks)
        {
            ProcessScript();

            _fieldController.Update(ticks);

            // only account for elapsed ticks after all agents have processed them
            // this prevent new agents from scripts being double-updated
            _elapsedTicks += ticks;

            _agents.RemoveAll(delegate(IAgent a)
            {
                if (a.IsActive) return false;
                a.DestroyResources();
                return true;
            });

            _overlayController.Update(ticks);
        }

        private void ProcessScript()
        {
            while (_scriptCommands.Count > 0 && _scriptCommands.Peek().Offset < _elapsedTicks)
            {
                var scriptCommand = _scriptCommands.Dequeue();

                var agent = _provider.CreateMob(scriptCommand.AgentTypeName, _map.GetPath(scriptCommand.PathName));

                _agents.Add(agent);

                // perform partial update to recover state from any delay in scripting
                agent.Update(_elapsedTicks - scriptCommand.Offset);
            }
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            _fieldController.CreateResources(createArgs);
            _overlayController.CreateResources(createArgs);
        }

        public void DestroyResources()
        {
            _fieldController.DestroyResources();
            _overlayController.DestroyResources();
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            // the FieldController draws the agents, so they are sited onto the map
            _fieldController.Draw(drawArgs, rotation);

            // the overlay draws second so that it is on top
            _overlayController.Draw(drawArgs, rotation);
        }

        #endregion
    }
}

