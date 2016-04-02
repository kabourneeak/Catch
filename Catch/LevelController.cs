using System;
using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Services;
using Catch.Win2d;

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
        private Map _map;

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

        public void Initialize(Vector2 size)
        {
            WindowSize = size;

            Score = StartScore;
            Lives = StartLives;

            _agents.Clear();

            CreateMap();
            CreatePaths();
            CreateTowers();
            SpawnBlock();

            _overlayController = new OverlayController();
            _overlayController.Initialize(WindowSize);

            _fieldController = new FieldController(_agents, _map);
            _fieldController.Initialize(WindowSize);
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

            foreach (var agent in _agents)
                agent.Update(ticks);

            _agents.RemoveAll(delegate(IAgent a)
            {
                if (a.IsActive) return false;
                a.DestroyResources();
                return true;
            });

            _fieldController.Update(ticks);
            _overlayController.Update(ticks);
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            foreach (var agent in _agents)
                agent.CreateResources(createArgs);

            _fieldController.CreateResources(createArgs);
            _overlayController.CreateResources(createArgs);
        }

        public void DestroyResources()
        {
            foreach (var agent in _agents)
                agent.DestroyResources();

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

        private void CreateMap()
        {
            _map = _provider.CreateMap();
            _map.Initialize(10, 19);
        }

        private void CreateTowers()
        {
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(4, 5)));
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(5, 5)));
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(4, 4)));
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(6, 5)));
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(5, 6)));

            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(4, 15)));
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(5, 15)));
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(4, 14)));
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(6, 15)));
            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(5, 16)));

            _agents.Add(_provider.CreateTower("GunTower", _map.GetTile(2, 1)));

            // fill the rest of the board
            foreach (var tile in _map)
            {
                if (!tile.HasTower())
                    _agents.Add(_provider.CreateTower("VoidTower", tile));
            }
        }

        private void CreatePaths()
        {
            var mapPath = new MapPath();

            var tile = _map.GetTile(0, 0);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.NorthEast);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.NorthEast);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.North);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.North);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.NorthEast);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.NorthEast);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.North);
            mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            mapPath.Add(tile);

            _map.AddPath("TestPath", mapPath);
        }

        private void SpawnBlock()
        {
            var block = _provider.CreateMob("BlockMob", _map.GetPath("TestPath"));
            _agents.Add(block);
        }

        #endregion
    }
}

