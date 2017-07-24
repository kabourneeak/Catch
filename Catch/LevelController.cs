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
    public class LevelController : IScreenController, ISimulationManager
    {
        private readonly FieldController _fieldController;
        private readonly OverlayController _overlayController;
        private readonly Random _rng = new Random();

        private readonly LevelState _level;
        private readonly IAgentProvider _agentProvider;
        private readonly List<IAgent> _agents;
        private readonly UpdateController _updatables;
        private readonly List<IDrawable> _drawables;

        #region Construction

        public LevelController(IConfig config, MapSerializationModel mapSerializationModel)
        {
            /*
             * Bootstrap simulation
             */

            var mapProvider = new BuiltinMapProvider(config);

            var map = mapProvider.CreateMap(mapSerializationModel.Rows, mapSerializationModel.Columns);

            _level = new LevelState(config, map);

            _agents = new List<IAgent>();
            _updatables = new UpdateController(this, _level);
            _drawables = new List<IDrawable>();

            _agentProvider = new BuiltinAgentProvider(config, this);

            InitializeMap(mapSerializationModel, map);
            InitializeEmitScript(mapSerializationModel);

            _overlayController = new OverlayController(_level, _agents);
            _fieldController = new FieldController(_level, _agents);
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
                };

                var tower = CreateTileAgent(tileModel.TowerName, towerArgs);
                this.Register(tower);
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

        private void InitializeEmitScript(MapSerializationModel mapSerializationModel)
        {
            /*
             * Process emit script
             */
            foreach (var emitScriptEntry in mapSerializationModel.EmitScript)
            {
                for (var i = 0; i < emitScriptEntry.Count; ++i)
                {
                    var agentArgs = new CreateAgentArgs()
                    {
                        Path = _level.Map.GetPath(emitScriptEntry.PathName),
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

            _updatables.Update(deviceTicks);
            _fieldController.Update(deviceTicks);

            // only account for elapsed ticks after all agents have processed them
            // this prevents new agents (those just emitted) from being double-updated

            _agents.RemoveAll(agent =>
            {
                if (agent.IsActive)
                    return false;

                //agent.DestroyResources();
                return true;
            });

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

        public void Register(IAgent agent)
        {
            this._agents.Add(agent);

            if (agent is IUpdatable)
            {
                this._updatables.Register((IUpdatable) agent);
            }

            if (agent is IDrawable)
            {
                this._drawables.Add((IDrawable) agent);
            }
        }

        public void Register(IUpdatable updatable)
        {
            this._updatables.Register(updatable);
        }

        public IAgent CreateAgent(string agentName, CreateAgentArgs createArgs)
        {
            return _agentProvider.CreateAgent(agentName, createArgs);
        }

        public ITileAgent CreateTileAgent(string agentName, CreateAgentArgs createArgs)
        {
            return (ITileAgent) _agentProvider.CreateAgent(agentName, createArgs);
        }

        public void Remove(IAgent agent)
        {
            agent.OnRemove();

            _agents.Remove(agent);
            _drawables.Remove(agent);

            // TODO remove from map / tile
        }

        #endregion
    }
}

