using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Catch.Models;
using Microsoft.Graphics.Canvas;

namespace Catch
{
    public enum GameState
    {
        Init, Title, Playing
    }

    public class GameStateChangedEventArgs : EventArgs
    {
        public GameState State;
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
        public Rect Size { get; private set; }
        private readonly Random _rng = new Random();

        //
        // Game State
        //
        public GameState State { get; set; }
        public int Score { get; private set; }
        public int Lives { get; private set; }
        private List<Block> _blocks;


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
            _blocks = new List<Block>();
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

            _blocks = new List<Block>();

            ChangeGameState(GameState.Playing);
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
            // nothing to update

            // raise GameState event so that someone calls our Initialize method.
            ChangeGameState(GameState.Init);
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
                var y = _rng.Next((int) Size.Height/Block.Size) * Block.Size;
                block.Position = new Vector2(0, y);
                block.Velocity = new Vector2(_rng.Next(1, 6), 0);
                block.Acceleration = new Vector2(0, 0);

                _blocks.Add(block);
            }
        }
    }
}

