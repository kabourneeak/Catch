﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Catch.Base;
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

        //
        // Game config
        //
        public Rect Size { get; private set; }
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
        private int _frameId;

        #region Event Handling

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

        #endregion

        #region Construction

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

        #endregion

        #region CatchGame API

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
            CreatePaths();
            CreateTowers();
            SpawnBlock();

            ChangeGameState(GameState.Playing);
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

        #region CreateResources

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            Debug.WriteLine("Mandatory CreateResources");
            CreateResources(resourceCreator, true);
        }

        private void CreateResources(ICanvasResourceCreator resourceCreator, bool mandatory)
        {
            var createArgs = new CreateResourcesArgs(resourceCreator, _frameId, mandatory);

            switch (AppliedState)
            {
                case GameState.Init:
                    break;
                case GameState.Title:
                    break;
                case GameState.Playing:
                    CreateResourcesPlaying(createArgs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateResourcesPlaying(CreateResourcesArgs createArgs)
        {
            foreach (var agent in _agents)
                agent.CreateResources(createArgs);
        }

        #endregion

        #region Draw

        public void Draw(CanvasDrawingSession ds)
        {
            _frameId += 1;

            CreateResources(ds, false);

            // prepare draw arguments
            var drawArgs = new DrawArgs(ds, ds.Transform, _frameId);

            // place origin in lower left
            drawArgs.Push(Matrix3x2.CreateTranslation(0, (float)Size.Height));
            drawArgs.Push(Matrix3x2.CreateScale(1.0f, -1.0f));
            
            // center w.r.t. map
            var mapSize = _map.Size;
            drawArgs.PushTranslation((float)((Size.Width - mapSize.X) / 2), (float)((Size.Height - mapSize.Y) / 2));

            switch (AppliedState)
            {
                case GameState.Init:
                    break;
                case GameState.Title:
                    break;
                case GameState.Playing:
                    DrawPlaying(drawArgs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawPlaying(DrawArgs drawArgs)
        {
            foreach (var agent in _agents)
                agent.Draw(drawArgs);
        }

        #endregion

        #region Update

        public void Update(float ticks)
        {
            // AppliedState is only updated here, and is meant ot keep Update, CreateResources, 
            // and Draw working on a consistent value
            AppliedState = State;

            switch (AppliedState)
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
                    throw new ArgumentOutOfRangeException();
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

            if (_frameId % 360 == 0)
                SpawnBlock();

            foreach (var agent in _agents)
            {
                agent.Update(ticks);
            }

            // TODO some method of remove agents that are dead
        }

        #endregion
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

