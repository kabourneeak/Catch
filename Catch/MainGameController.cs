using System;
using System.IO;
using System.Numerics;
using Catch.Graphics;
using CatchLibrary.Serialization;
using Newtonsoft.Json;

namespace Catch
{
    /// <summary>
    /// Manages the top level state of the overall application, whether it is sitting
    /// at the title screen or playing a level. Bootstraps the game into a running 
    /// state.
    /// </summary>
    public class MainGameController : IGameController
    {
        private bool _forceCreateResources;
        public Vector2 WindowSize { get; private set; }
        public GameState State { get; set; }
        private GameState AppliedState { get; set; }
        private GameStateArgs RequestedState { get; set; }

        private IGameController CurrentController { get; set; }

        public MainGameController()
        {
            // start up in Initialize state;
            // this will be replaced by bootstrapping when the canvas controller 
            // calls our Initialize method
            State = GameState.Initializing;
            CurrentController = new NilGameController();
        }

        #region IGameController Implementation

        public event GameStateChangedHandler GameStateChangeRequested;

        public void Initialize(Vector2 size, GameStateArgs args)
        {
            WindowSize = size;

            // Bootstrap initial game state
            // TODO don't read a hard-coded file name
            var filename = "MapOne.json";
            var mapModel = JsonConvert.DeserializeObject<MapModel>(File.ReadAllText(filename));

            OnGameStateChangeRequest(this, new GameStateArgs(GameState.PlayMap, mapModel));
        }

        private void OnGameStateChangeRequest(object sender, GameStateArgs args)
        {
            RequestedState = args;
        }

        private void HandoffController()
        {
            // there is only one controller per state, so if the state is the same, don't do anything
            if (State == RequestedState.State)
                return;

            /*
             * teardown old controller
             */

            CurrentController?.DestroyResources();

            /*
             * create new controller
             */

            var requestedController = CreateGameController(RequestedState.State);
            requestedController.GameStateChangeRequested += OnGameStateChangeRequest;

            // feed initializing events to new controller
            requestedController.Initialize(WindowSize, RequestedState);
            _forceCreateResources = true;

            CurrentController = requestedController;
        }

        private IGameController CreateGameController(GameState state)
        {
            switch (state)
            {
                case GameState.Initializing:
                    return new NilGameController();
                case GameState.PlayMap:
                    return new LevelController();
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        #endregion

        #region IGraphicsComponent Implementation

        public void Update(float ticks)
        {
            if (RequestedState != null)
            {
                HandoffController();
                RequestedState = null;
            }

            // AppliedState is only updated here, and is meant to keep Update, CreateResources, 
            // and Draw working on a consistent value
            AppliedState = State;

            CurrentController?.Update(ticks);
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            // CreateResources must be mandatory when there is a controller change, as the new controller
            // will not have received an "organic" CreateResources from the Win2d device.
            if (_forceCreateResources)
            {
                createArgs.SetMandatory();
                _forceCreateResources = false;
            }

            CurrentController?.CreateResources(createArgs);
        }

        public void DestroyResources()
        {
            CurrentController?.DestroyResources();
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            CurrentController?.Draw(drawArgs, rotation);
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(Vector2 panDelta)
        {
            CurrentController?.PanBy(panDelta);
        }

        public void ZoomToPoint(Vector2 viewCoords, float zoomDelta)
        {
            CurrentController?.ZoomToPoint(viewCoords, zoomDelta);
        }

        public void Resize(Vector2 size)
        {
            // store for ourselves so we can forward when controllers changeover
            WindowSize = size;

            CurrentController?.Resize(size);
        }

        public void Hover(Vector2 viewCoords) => CurrentController?.Hover(viewCoords);

        #endregion
    }
}
