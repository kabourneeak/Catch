using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Catch.Graphics;
using CatchLibrary.Serialization;
using Newtonsoft.Json;

namespace Catch.Services
{
    /// <summary>
    /// Manages the active screens for the application, whether it is sitting
    /// at the title screen or playing a level. 
    /// 
    /// Also, through its IScreenController interface, delegates device events as 
    /// needed to active screens. 
    /// 
    /// Also, Bootstraps the game into a running state.
    /// </summary>
    public class DelegatingScreenManager : IScreenManager, IScreenController
    {
        private const string CfgMapsFolder = nameof(DelegatingScreenManager) + ".MapsFolder";
        private const string CfgInitialMap = nameof(DelegatingScreenManager) + ".InitialMap";

        private bool _forceCreateResources;
        private CompiledConfig Config { get; }
        private Vector2 WindowSize { get; set; }
        private GameState State { get; set; }
        private GameState AppliedState { get; set; }
        private GameStateArgs RequestedState { get; set; }

        private List<IScreenController> CurrentScreens { get; set; }

        public DelegatingScreenManager()
        {
            // start up in Initialize state;
            // this will be replaced by bootstrapping when the canvas controller 
            // calls our Initialize method
            State = GameState.Initializing;

            CurrentScreens = new List<IScreenController>
            {
                new NilScreenController()
            };

            Config = new CompiledConfig();
        }

        #region IScreenManager Implementation

        public void RequestScreen(GameStateArgs args)
        {
            // store request, it will be processed on next loop
            RequestedState = args;
        }

        public void CloseScreen(IScreenController screen)
        {
            if (screen != CurrentScreens.Last())
                throw new ArgumentException("Only the top screen can be closed.");

            screen.DestroyResources();
            CurrentScreens.Remove(screen);
        }

        private void HandoffController()
        {
            /*
             * TODO Rewrite the HandoffController concept
             * 
             * This code is held over from when we had individual State controllers, instead
             * of a stack of screens that co-exist.
             * 
             * RequestScreen(), GameStateArgs, GameState should all be revisited.
             */

            // there is only one controller per state, so if the state is the same, don't do anything
            if (State == RequestedState.State)
                return;

            /*
             * create new controller
             */

            var requestedController = CreateScreenController(RequestedState.State);

            // feed initializing events to new controller
            requestedController.Initialize(WindowSize, RequestedState);
            _forceCreateResources = true;

            CurrentScreens.Add(requestedController);

            /*
             * handoff complete
             */
            State = RequestedState.State;
        }

        private IScreenController CreateScreenController(GameState state)
        {
            switch (state)
            {
                case GameState.Initializing:
                    return new NilScreenController();
                case GameState.PlayMap:
                    return new LevelController(Config);
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        #endregion

        #region IScreenController Implementation

        public void Initialize(Vector2 size, GameStateArgs args)
        {
            WindowSize = size;

            // Bootstrap initial game state
            MapModel mapModel;

            try
            {
                var appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var mapsFolder =
                    appInstalledFolder.GetFolderAsync(Config.GetString(CfgMapsFolder))
                        .GetAwaiter()
                        .GetResult();

                var initialMapPath = Path.Combine(mapsFolder.Path, Config.GetString(CfgInitialMap));
                var initialMapData = File.ReadAllText(initialMapPath);

                mapModel = JsonConvert.DeserializeObject<MapModel>(initialMapData);
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new IOException("Could not read initial map", e);
            }

            RequestScreen(new GameStateArgs(GameState.PlayMap, mapModel));
        }

        public bool AllowPredecessorUpdate() => false;

        public bool AllowPredecessorDraw() => false;

        public bool AllowPredecessorInput() => false;

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

            foreach (var screen in CurrentScreens.ReverseIterator())
            {
                screen.Update(ticks);

                if (!screen.AllowPredecessorUpdate())
                    break;
            }
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

            // CreateResources is delegated to all active screens
            foreach (var screenController in CurrentScreens)
                screenController.CreateResources(createArgs);
        }

        public void DestroyResources()
        {
            // DestroyResources is delegated to all active screens
            foreach (var screenController in CurrentScreens)
                screenController.DestroyResources();
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            int deepestScreen;

            for (deepestScreen = CurrentScreens.Count - 1; deepestScreen >= 0; --deepestScreen)
                if (!CurrentScreens[deepestScreen].AllowPredecessorDraw())
                    break;

            for (var screen = deepestScreen; screen < CurrentScreens.Count; ++screen)
                CurrentScreens[screen].Draw(drawArgs, rotation);
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(PanByEventArgs eventArgs)
        {
            DelegateInputEvent(screen => screen.PanBy(eventArgs), eventArgs);
        }

        public void ZoomToPoint(ZoomToPointEventArgs eventArgs)
        {
            DelegateInputEvent(screen => screen.ZoomToPoint(eventArgs), eventArgs);
        } 

        public void Resize(Vector2 size)
        {
            // store for ourselves so we can forward when controllers changeover
            WindowSize = size;

            foreach (var screen in CurrentScreens)
                screen.Resize(size);
        }

        public void Hover(HoverEventArgs eventArgs)
        {
            DelegateInputEvent(screen => screen.Hover(eventArgs), eventArgs);
        }

        public void Touch(TouchEventArgs eventArgs)
        {
            DelegateInputEvent(screen => screen.Touch(eventArgs), eventArgs);
        }

        public void KeyPress(KeyPressEventArgs eventArgs)
        {
            DelegateInputEvent(screen => screen.KeyPress(eventArgs), eventArgs);
        }

        private void DelegateInputEvent(Action<IScreenController> inputAction, EventArgsBase eventArgs)
        {
            foreach (var screen in CurrentScreens.ReverseIterator())
            {
                inputAction(screen);

                if (!screen.AllowPredecessorInput() || eventArgs.Handled)
                    break;
            }
        }

        #endregion
    }
}
