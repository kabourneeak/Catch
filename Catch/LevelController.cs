using System;
using System.Collections.Generic;
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
    public class LevelController : IScreenController
    {
        private readonly FieldController _fieldController;
        private readonly OverlayController _overlayController;
        private readonly Random _rng = new Random();

        private readonly IConfig _config;
        private readonly LevelState _level;
        private readonly IAgentProvider _agentProvider;
        private readonly IMapProvider _mapProvider;
        private readonly Queue<ScriptCommand> _scriptCommands;

        #region Construction

        public LevelController(IConfig config, MapSerializationModel mapSerializationModel)
        {
            _config = config;

            _agentProvider = new BuiltinAgentProvider(_config);
            _mapProvider = new BuiltinMapProvider(_config);

            var map = _mapProvider.CreateMap(mapSerializationModel.Rows, mapSerializationModel.Columns);

            _level = new LevelState(config, map);

            InitializeMap(mapSerializationModel, map);
            _scriptCommands = InitializeEmitScript(mapSerializationModel);

            _overlayController = new OverlayController(_level, _level.Agents);
            _fieldController = new FieldController(_level, _level.Agents);
        }

        private void InitializeMap(MapSerializationModel mapSerializationModel, MapModel map)
        {
            /*
             * Process tile models
             */
            foreach (var tile in map)
            {
                var tileModel = mapSerializationModel.Tiles.GetHex(tile.Coords);
                var towerArgs = new CreateAgentArgs()
                {
                    Tile = tile,
                    StateModel = _level
                };

                var tower = _agentProvider.CreateAgent(tileModel.TowerName, towerArgs);
                _level.AddAgent(tower);
            }

            /*
             * Process paths
             */
            foreach (var pathModel in mapSerializationModel.Paths)
            {
                var mapPath = new MapPath();
                mapPath.Name = pathModel.PathName;

                foreach (var pathStep in pathModel.PathSteps)
                {
                    mapPath.Add(map.GetHex(pathStep.Coords));
                }

                map.AddPath(mapPath);
            }
        }

        private Queue<ScriptCommand> InitializeEmitScript(MapSerializationModel mapSerializationModel)
        {
            /*
             * Process emit script
             */
            var scriptCommandList = new List<ScriptCommand>();

            foreach (var emitScriptEntry in mapSerializationModel.EmitScript)
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

            var queue = new Queue<ScriptCommand>(scriptCommandList);

            return queue;
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

        #region IUpdatable Implementation

        private float _elapsedTicks = 0.0f;

        public void Update(float ticks)
        {
            ProcessScript();

            _fieldController.Update(ticks);

            // only account for elapsed ticks after all agents have processed them
            // this prevents new agents (those just emitted) from being double-updated
            _elapsedTicks += ticks;

            _level.Agents.RemoveAll(agent =>
            {
                if (agent.IsActive)
                    return false;

                //agent.DestroyResources();
                return true;
            });

            _overlayController.Update(ticks);
        }

        private void ProcessScript()
        {
            while (_scriptCommands.Count > 0 && _scriptCommands.Peek().Offset < _elapsedTicks)
            {
                var scriptCommand = _scriptCommands.Dequeue();

                var agentArgs = new CreateAgentArgs()
                {
                    Path = _level.Map.GetPath(scriptCommand.PathName),
                    StateModel = _level
                };

                var agent = _agentProvider.CreateAgent(scriptCommand.AgentTypeName, agentArgs);

                _level.Agents.Add(agent);

                // perform partial update to recover state from any delay in scripting
                agent.Update(_elapsedTicks - scriptCommand.Offset);
            }
        }

        #endregion

        #region IGraphicsComponent Implementation

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
            // the FieldController draws the agents, so they are sited onto the map
            _fieldController.Draw(drawArgs, rotation);

            // the overlay draws second so that it is on top
            _overlayController.Draw(drawArgs, rotation);
        }

        #endregion
    }
}

