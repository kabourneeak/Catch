using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Catch.Base;
using Catch.Models;
using Catch.Services;
using Catch.Win2d;
using Microsoft.Graphics.Canvas;

namespace Catch
{
    /// <summary>
    /// Model Controller for game state
    /// </summary>
    public class CatchGame
    {
        private const int StartLives = 3;
        private const int StartScore = 0;
        private const int ScoreIncrement = 10;

        private const int BlockMax = 3;
        private const int BlockSpawnRate = 10;

        //
        // Game config
        //
        public Rect Size { get; private set; }
        private readonly Random _rng = new Random();

        //
        // Game State
        //
        public GameState State { get; set; }
        public int Score { get; private set; }
        public int Lives { get; private set; }

        private IConfig _config;
        private Win2DProvider _provider;
        private readonly List<IAgent> _agents;
        private Map _map;
        private int _frameId;

        // testing
        private MapPath _mapPath;

        //
        // event handling
        //
        public delegate void GameStateChangedHandler(object sender, GameStateChangedEventArgs e);
        public event GameStateChangedHandler GameStateChanged;

        protected virtual void RaiseGameStateChanged()
        {
            if (GameStateChanged != null)
            { 
                GameStateChanged(this, new GameStateChangedEventArgs {State = this.State});
            }
        }

        private void ChangeGameState(GameState newState)
        {
            State = newState;
            RaiseGameStateChanged();
        }

        //
        // construction
        //
        public CatchGame()
        {
            State = GameState.Init;

            AssembleServices();

            _agents = new List<IAgent>();
        }

        private void AssembleServices()
        {
            _config = new CompiledConfig();
            _provider = new Win2DProvider(_config);
        }

        public void Initialize(Rect size)
        {
            Size = size;

            ChangeGameState(GameState.Title);
        }

        public void Resize(Rect size)
        {
            Size = size;
        }

        public void StartGame()
        {
            if (State != GameState.Title)
                throw new InvalidOperationException(string.Format("Cannot StartGame() from the {0} state", State));

            Score = StartScore;
            Lives = StartLives;

            _agents.Clear();

            CreateMap();
            CreateTowers();
            CreatePath();

            ChangeGameState(GameState.Playing);
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

        }

        private void CreatePath()
        {
            _mapPath = new MapPath();

            var tile = _map.GetTile(0, 0);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.South);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.South);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.NorthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.North);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.North);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.SouthEast);
            _mapPath.Add(tile);

            tile = _map.GetNeighbour(tile, TileDirection.NorthEast);
            _mapPath.Add(tile);

        }

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            Debug.WriteLine("Mandatory CreateResources");
            CreateResources(resourceCreator, true);
        }

        private void CreateResources(ICanvasResourceCreator resourceCreator, bool mandatory)
        {
            var createArgs = new CreateResourcesArgs(resourceCreator, _frameId, mandatory);

            foreach (var agent in _agents)
                agent.CreateResources(createArgs);
        }

        public void Draw(CanvasDrawingSession ds)
        {
            CreateResources(ds, false);

            var mapSize = _map.Size;
            
            var drawArgs = new DrawArgs(ds, ds.Transform, ++_frameId);

            drawArgs.PushTranslation((float) ((Size.Width - mapSize.X) / 2), (float) ((Size.Height - mapSize.Y) / 2));

            foreach (var agent in _agents)
                agent.Draw(drawArgs);
        }

        public void Update(float ticks)
        {
            switch (State)
            {
                case GameState.Playing:
                    UpdatePlaying(ticks);
                    break;
                case GameState.Title:
                    UpdateTitle(ticks);
                    break;
                case GameState.Init:
                    UpdateInit(ticks);
                    break;
                default:
                    throw new ArgumentException("Unhandled game state");
            }
        }

        private void UpdateInit(float ticks)
        {
            // nothing to update

            // raise GameState event so that someone calls our Initialize method.
            ChangeGameState(GameState.Init);
        }

        private void UpdateTitle(float ticks)
        {
            // Nothing to do at the title screen for now
        }

        private void UpdatePlaying(float ticks)
        {
            Score += ScoreIncrement;

            SpawnBlock();

            foreach (var agent in _agents)
            {
                agent.Update(ticks);
            }

            // TODO some method of remove agents that are dead
        }

        private void CreateMap()
        {
            _map = _provider.CreateMap();
            _map.Initialize(10, 19);

            for (var c = 0; c < 2; ++c)
            {
                for (var r = 0; r < 2; ++r)
                {
                    var tile = _map.GetTile(r, c);
                    var tower = new VoidTower(tile, _config);
                    tile.SetTower(tower);
                }
            }
        }

        private void SpawnBlock()
        {
            if (_rng.NextDouble() < (3 / 60.0))
            {
                var block = _provider.CreateMob("BlockMob", _mapPath);
                _agents.Add(block);
            }
        }
    }

    public enum GameState
    {
        Init, Title, Playing
    }

    public class GameStateChangedEventArgs : EventArgs
    {
        public GameState State;
    }
}

