﻿using System.Numerics;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Catch.Graphics;
using Catch.Services;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Catch
{
    /// <summary>
    /// Handles low-level input, timing, and other "device-level" things such as the view size
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly DelegatingScreenManager _gameManager;
        private int _frameId;
        private const float TicksPerSecond = 60.0f;

        public MainPage()
        {
            InitializeComponent();

            _frameId = 0;

            _gameManager = new DelegatingScreenManager();
            _gameManager.Initialize(new Vector2((float)cvs.Size.Width, (float)cvs.Size.Height), null);
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            // attach keyboard events
            Window.Current.CoreWindow.KeyDown += KeyDown_UIThread;
        }

        #region Animation Loop

        private CoreIndependentInputSource _inputDevice;
        private GestureRecognizer _gestureRecognizer;

        private void OnGameLoopStarting(ICanvasAnimatedControl sender, object args)
        {
            _gestureRecognizer = new GestureRecognizer();
            _gestureRecognizer.GestureSettings = GestureSettings.ManipulationTranslateX |
                                                 GestureSettings.ManipulationTranslateY |
                                                 GestureSettings.ManipulationScale;

            _gestureRecognizer.ManipulationStarted += gestureRecognizer_ManipulationStarted;
            _gestureRecognizer.ManipulationUpdated += gestureRecognizer_ManipulationUpdated;
            _gestureRecognizer.ManipulationCompleted += gestureRecognizer_ManipulationCompleted;

            //
            // When the GestureRecognizer goes into intertia mode (ie after the pointer is released)
            // we want it to generate ManipulationUpdated events in sync with the game loop's Update.
            // We do this by disabling AutoProcessIntertia and explicitly calling ProcessInertia() 
            // from the Update.
            //
            _gestureRecognizer.InertiaTranslationDeceleration = -5.0f;
            _gestureRecognizer.AutoProcessInertia = false;

            _inputDevice =
                cvs.CreateCoreIndependentInputSource(CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Touch |
                                                     CoreInputDeviceTypes.Pen);

            _inputDevice.PointerPressed += OnPointerPressed;
            _inputDevice.PointerReleased += OnPointerReleased;
            _inputDevice.PointerWheelChanged += OnPointerWheelChanged;
            _inputDevice.PointerMoved += OnPointerMoved;
        }

        private void OnGameLoopStopped(ICanvasAnimatedControl sender, object args)
        {
            _inputDevice.PointerPressed -= OnPointerPressed;
            _inputDevice.PointerReleased -= OnPointerReleased;
            _inputDevice.PointerWheelChanged -= OnPointerWheelChanged;
            _inputDevice.PointerMoved -= OnPointerMoved;

            _gestureRecognizer.ManipulationStarted -= gestureRecognizer_ManipulationStarted;
            _gestureRecognizer.ManipulationUpdated -= gestureRecognizer_ManipulationUpdated;
            _gestureRecognizer.ManipulationCompleted -= gestureRecognizer_ManipulationCompleted;
        }

        private void OnCreateResources(ICanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            _frameId += 1;

            var resourceCreator = sender.Device;
            var createArgs = new CreateResourcesArgs(resourceCreator, _frameId, true);

            _gameManager.CreateResources(createArgs);
        }

        private void OnUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            if (_inManipulation)
                _gestureRecognizer?.ProcessInertia();

            var elapsedMs = args.Timing.ElapsedTime.Milliseconds;
            var elapsedTicks = TicksPerSecond * elapsedMs / 1000.0f;

            _gameManager.Update(elapsedTicks);
        }

        private void OnDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            _frameId += 1;

            // send non-mandatory CreateResources
            // TODO we might be able to remove this by making other resources self-initializing
            var resourceCreator = sender.Device;
            var createArgs = new CreateResourcesArgs(resourceCreator, _frameId, false);

            _gameManager.CreateResources(createArgs);

            // send draw
            var ds = args.DrawingSession;
            var drawArgs = new DrawArgs(ds, ds.Transform, _frameId);

            _gameManager.Draw(drawArgs, 0.0f);
        }

        #endregion

        #region Viewport Size

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            //
            // update layout
            //

            // animated area
            cvs.Height = args.NewSize.Height;
            cvs.Width = args.NewSize.Width;
        }

        private void cvs_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            var cvsSize = cvs.Size;

            _gameManager.Resize(new Vector2((float)cvsSize.Width, (float)cvsSize.Height));
        }

        #endregion

        #region Pan and Zoom

        private bool _inManipulation = false;

        private void OnPointerPressed(object sender, PointerEventArgs args)
        {
            _gestureRecognizer.ProcessDownEvent(args.CurrentPoint);

            if (_inManipulation)
            {
                // do nothing
            }
            else if (args.CurrentPoint.Properties.IsLeftButtonPressed)
            {
                _gameManager.Touch(new TouchEventArgs(args.CurrentPoint.Position.ToVector2(), args.KeyModifiers));
            }

            args.Handled = true;
        }

        private void OnPointerMoved(object sender, PointerEventArgs args)
        {
            _gestureRecognizer.ProcessMoveEvents(args.GetIntermediatePoints());

            if (_inManipulation)
            {
                // do nothing
            }
            else if (!args.CurrentPoint.Properties.IsLeftButtonPressed)
            {
                _gameManager.Hover(new HoverEventArgs(args.CurrentPoint.Position.ToVector2(), args.KeyModifiers));
            }

            args.Handled = true;
        }

        private void OnPointerReleased(object sender, PointerEventArgs args)
        {
            _gestureRecognizer.ProcessUpEvent(args.CurrentPoint);
            args.Handled = true;
        }

        private void OnPointerWheelChanged(object sender, PointerEventArgs args)
        {
            var coords = args.CurrentPoint.Position.ToVector2();
            var wheelTicks = args.CurrentPoint.Properties.MouseWheelDelta;
            wheelTicks = wheelTicks / 120;

            _gameManager.ZoomToPoint(new ZoomToPointEventArgs(coords, wheelTicks * 0.1f));
        }

        private void gestureRecognizer_ManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {
            _inManipulation = true;
        }

        private void gestureRecognizer_ManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            var screenDelta = Vector2.Zero;

            screenDelta.X = (float) args.Delta.Translation.X;
            screenDelta.Y = (float) args.Delta.Translation.Y * -1.0f;
            _gameManager.PanBy(new PanByEventArgs(screenDelta));

            _gameManager.ZoomToPoint(new ZoomToPointEventArgs(args.Position.ToVector2(), args.Delta.Scale - 1.0f));
        }

        private void gestureRecognizer_ManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
            _inManipulation = false;
        }

        #endregion

        #region Keyboard Input

        private void KeyDown_UIThread(CoreWindow sender, KeyEventArgs args)
        {
            var key = args.VirtualKey;
            
            args.Handled = true;

            // Schedule to run on game loop
            var action = cvs.RunOnGameLoopThreadAsync(() => _gameManager.KeyPress(new KeyPressEventArgs(key)));
        }

        #endregion
    }
}
