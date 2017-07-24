using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Catch.Graphics;

namespace Catch.Services
{
    /// <summary>
    /// Manages the active screens for the application, whether it is sitting
    /// at the title screen or playing a level. 
    /// 
    /// Also, through its IScreenController interface, delegates device events as 
    /// needed to active screens. 
    /// </summary>
    public class DelegatingScreenManager : IScreenManager, IScreenController
    {
        private bool _forceCreateResources;
        private Vector2 WindowSize { get; set; }
        private IScreenController RequestedScreen { get; set; }

        private List<IScreenController> CurrentScreens { get; }

        public DelegatingScreenManager()
        {
            CurrentScreens = new List<IScreenController>
            {
                new NilScreenController()
            };
        }

        #region IScreenManager Implementation

        public void RequestScreen(IScreenController screen)
        {
            // store request, it will be processed on next loop
            RequestedScreen = screen;
        }

        public void CloseScreen(IScreenController screen)
        {
            if (screen != CurrentScreens.Last())
                throw new ArgumentException("Only the top screen can be closed.");

            screen.DestroyResources();
            CurrentScreens.Remove(screen);
        }

        #endregion

        #region IScreenController Implementation

        public void Initialize(Vector2 size)
        {
            WindowSize = size;
        }

        public bool AllowPredecessorUpdate() => false;

        public bool AllowPredecessorDraw() => false;

        public bool AllowPredecessorInput() => false;

        public void Update(float deviceTicks)
        {
            if (RequestedScreen != null)
            {
                // feed initializing events to new screen controller
                RequestedScreen.Initialize(WindowSize);
                _forceCreateResources = true;

                CurrentScreens.Add(RequestedScreen);

                RequestedScreen = null;
            }

            foreach (var screen in CurrentScreens.ReverseIterator())
            {
                screen.Update(deviceTicks);

                if (!screen.AllowPredecessorUpdate())
                    break;
            }
        }

        #endregion

        #region IGraphicsComponent Implementation

        public void CreateResources(CreateResourcesArgs args)
        {
            // CreateResources must be mandatory when there is a controller change, as the new controller
            // will not have received an "organic" CreateResources from the Win2d device.
            if (_forceCreateResources)
            {
                args.SetMandatory();
                _forceCreateResources = false;
            }

            // CreateResources is delegated to all active screens
            foreach (var screenController in CurrentScreens)
                screenController.CreateResources(args);
        }

        public void DestroyResources()
        {
            // DestroyResources is delegated to all active screens
            foreach (var screenController in CurrentScreens)
                screenController.DestroyResources();
        }

        #endregion

        #region IDrawable Implementation

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
