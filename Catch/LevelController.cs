using System;
using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Services;
using Catch.Win2d;

namespace Catch
{
    /// <summary>
    /// Model Controller for game state
    /// </summary>
    public class LevelController : IGameController
    {
        private const int StartLives = 3;
        private const int StartScore = 0;
        private const int ScoreIncrement = 10;

        //
        // Game config
        //
        public Vector2 WindowSize { get; private set; }
        public float Zoom { get; private set; }
        public Vector2 Pan { get { return _pan; } }

        private readonly Random _rng = new Random();

        //
        // Game State
        //
        public GameState State { get; set; }
        private GameState AppliedState { get; set; }
        public int Score { get; private set; }
        public int Lives { get; private set; }

        private IConfig _config;
        private Win2DProvider _provider;
        private readonly List<IAgent> _agents;
        private Map _map;
        private Matrix3x2 _mapTransform;
        private Vector2 _pan;

        #region Event Handling

        public event GameStateChangedHandler GameStateChanged;

        protected virtual void RaiseGameStateChanged()
        {
            if (GameStateChanged != null)
            { 
                GameStateChanged(this, new GameStateChangeRequestEventArgs {State = this.State});
            }
        }

        private void ChangeGameState(GameState newState)
        {
            State = newState;
            RaiseGameStateChanged();
        }

        #endregion

        #region Construction

        public LevelController()
        {
            State = GameState.Initializing;

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
            Zoom = 1.0f;
            _pan = Vector2.Zero;

            Score = StartScore;
            Lives = StartLives;

            _agents.Clear();

            CreateMap();
            CreatePaths();
            CreateTowers();

            Zoom = 1.0f;
            _pan.X = (WindowSize.X - _map.Size.X) / 2.0f;
            _pan.Y = WindowSize.Y * -1.0f + (WindowSize.Y - _map.Size.Y) / 2.0f;
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(Vector2 panDelta)
        {
            _pan = Vector2.Add(_pan, Vector2.Multiply(panDelta, 1.0f / Zoom));
        }

        public void ZoomToPoint(Vector2 viewCoords, float zoomDelta)
        {
            var newZoom = Math.Max(0.4f, Math.Min(2.0f, Zoom + zoomDelta));

            var zoomCenter = TranslateToMap(viewCoords);
            _pan = Vector2.Add(_pan, zoomCenter);
            _pan = Vector2.Multiply(_pan, Zoom / newZoom);
            _pan = Vector2.Subtract(_pan, zoomCenter);

            Zoom = newZoom;
        }

        public void Resize(Vector2 size)
        {
            WindowSize = size;
        }

        #endregion

        public Vector2 TranslateToMap(Vector2 coords)
        {
            return Vector2.Transform(coords, _mapTransform);
        }

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
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            foreach (var agent in _agents)
                agent.CreateResources(createArgs);
        }

        public void DestroyResources()
        {
            foreach (var agent in _agents)
                agent.DestroyResources();
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            // apply view matrix
            drawArgs.PushScale(1.0f, -1.0f);
            drawArgs.PushScale(Zoom);
            drawArgs.PushTranslation(_pan);

            // calculate viewport transform, used for zoom/pan
            Matrix3x2.Invert(drawArgs.CurrentTransform, out _mapTransform);

            foreach (var agent in _agents)
                agent.Draw(drawArgs, 0.0f);
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

