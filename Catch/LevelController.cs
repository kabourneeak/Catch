using System;
using System.Collections.Generic;
using System.Numerics;
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
        private const int StartLives = 3;
        private const int StartScore = 0;
        private const int ScoreIncrement = 10;

        private FieldController _fieldController;
        private OverlayController _overlayController;
        private readonly Random _rng = new Random();

        //
        // Game config
        //
        private Vector2 WindowSize { get; set; }

        //
        // Game State
        //
        private int Score { get; set; }
        private int Lives { get; set; }

        private readonly IConfig _config;
        private readonly Win2DProvider _provider;
        private readonly List<IAgent> _agents;
        private Map.Map _map;

        #region Construction

        public LevelController()
        {
            _config = new CompiledConfig();
            _provider = new Win2DProvider(_config);
            _agents = new List<IAgent>();
        }

        #endregion

        #region IGameController Implementation

        public event GameStateChangedHandler GameStateChangeRequested;

        public void Initialize(Vector2 size, GameStateArgs args)
        {
            WindowSize = size;

            Score = StartScore;
            Lives = StartLives;

            _agents.Clear();

            InitializeMap(args.MapModel);

            SpawnBlock();

            _overlayController = new OverlayController();
            _overlayController.Initialize(WindowSize);

            _fieldController = new FieldController(_agents, _map);
            _fieldController.Initialize(WindowSize);
        }

        private void InitializeMap(MapModel mapModel)
        {
            _map = _provider.CreateMap(mapModel.Rows, mapModel.Columns);

            /*
             * Process tile models
             */
            foreach (var tile in _map)
            {
                var tileModel = mapModel.Tiles.GetHex(tile.Row, tile.Column);
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
                    mapPath.Add(_map.GetHex(pathStep.Row, pathStep.Column));
                }

                _map.AddPath(mapPath);
            }
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
            WindowSize = size;

            _fieldController.Resize(size);
            _overlayController.Resize(size);
        }

        #endregion

        #region IGraphicsComponent Implementation

        private float _spawnTimer = SPAWN_TICKS;
        private const float SPAWN_TICKS = 360.0f;

        public void Update(float ticks)
        {
            Score += ScoreIncrement;

            _spawnTimer -= ticks;
            if (_spawnTimer < 0.0f)
            {
                SpawnBlock();
                _spawnTimer = SPAWN_TICKS;
            }

            _fieldController.Update(ticks);

            _agents.RemoveAll(delegate(IAgent a)
            {
                if (a.IsActive) return false;
                a.DestroyResources();
                return true;
            });

            _overlayController.Update(ticks);
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

        #region Test Environment Setup

        private void SpawnBlock()
        {
            var block = _provider.CreateMob("BlockMob", _map.GetPath("testPath"));
            _agents.Add(block);
        }

        #endregion
    }
}

