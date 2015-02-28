using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using Windows.Foundation;
using Catch.Models;
using Microsoft.Graphics.Canvas;

namespace Catch
{
    public enum GameState
    {
        Init, Title, Playing
    }


    /// <summary>
    /// Model Controller for game state
    /// </summary>
    public class CatchGame
    {
        private const int StartLives = 3;
        private const int StartScore = 0;
        private const int ScoreIncrement = 10;

        private const int BlockMax = 100;
        private const int BlockSpawnRate = 10;

        //
        // Game config
        //
        private SynchronizationContext _ownerContext;
        private GameStateChangedHandler _ownerHandler;
        public Rect Size { get; private set; }
        private Random _rng = new Random();

        //
        // Game State
        //
        public GameState State { get; set; }
        public int Score { get; private set; }
        public int Lives { get; private set; }
        private List<Block> _blocks;


        public delegate void GameStateChangedHandler(GameState state);

        protected virtual void RaiseGameStateChanged()
        {
            _ownerContext.Post(delegate(object state)
            {
                // _ownerHandler((GameState)state);
                Debug.WriteLine("yo");
            }, State);
        }

        public CatchGame(SynchronizationContext ownerContext, GameStateChangedHandler ownerHandler)
        {
            _ownerContext = ownerContext;
            _ownerHandler = ownerHandler;

            State = GameState.Init;
            _blocks = new List<Block>();
        }

        public void Initialize(Rect size)
        {
            Size = size;

            ChangeGameState(GameState.Title);
        }

        private void ChangeGameState(GameState newState)
        {
            State = newState;
            RaiseGameStateChanged();
        }

        public void StartGame()
        {
            if (State != GameState.Title)
                throw new InvalidOperationException(string.Format("Cannot StartGame() from the {0} state", State));

            Score = StartScore;
            Lives = StartLives;

            _blocks = new List<Block>();
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            foreach (var drawable in _blocks)
            {
                drawable.Draw(drawingSession);
            }
        }

        public void Update()
        {
            switch (State)
            {
                case GameState.Playing:
                    UpdatePlaying();
                    break;
                case GameState.Title:
                    UpdateTitle();
                    break;
                case GameState.Init:
                    UpdateInit();
                    break;
                default:
                    throw new ArgumentException("Unhandled game state");
            }
        }

        private void UpdateInit()
        {
            // wait for someone to call a method that will move us out of the Init state.
            RaiseGameStateChanged();
        }

        private void UpdateTitle()
        {
            // Nothing to do at the title screen for now
        }

        private void UpdatePlaying()
        {
            Score += ScoreIncrement;

            DestroyBlocks();
            SpawnBlocks();

            foreach (var block in _blocks)
                block.Update();

        }

        private void DestroyBlocks()
        {
            _blocks.RemoveAll(block => block.Position.X > Size.Width);
        }

        private void SpawnBlocks()
        {
            for (var i = 0; i < _rng.Next(BlockSpawnRate); ++i)
            {
                if (!(_blocks.Count < BlockMax))
                    break;

                var block = new Block();
                block.Position = new Vector2(0, _rng.Next((int)Size.Height));
                block.Velocity = new Vector2(_rng.Next(1, 6), 0);
                block.Acceleration = new Vector2(0, 0);

                _blocks.Add(block);
            }
        }
    }
}

