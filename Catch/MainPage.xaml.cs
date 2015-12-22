using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Catch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CatchGame _game;

        public MainPage()
        {
            InitializeComponent();

            _game = new CatchGame();
            _game.GameStateChanged += GameStateHandler;
        }

        #region " Game Loop "

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

        private void OnCreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            _game.CreateResources(sender.Device);
        }

        private void OnUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            if (_inManipulation && _gestureRecognizer != null)
                _gestureRecognizer.ProcessInertia();

            // TODO make the number of ticks depend on wall time elapsed, maybe some other gamespeed setting?
            _game.Update(1);
        }

        private void OnDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            _game.Draw(args.DrawingSession);
        }

        #endregion

        private void GameStateHandler(object sender, GameStateChangedEventArgs args)
        {
            if (!Dispatcher.HasThreadAccess)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate { GameStateHandler(sender, args); })
                    .Forget();
                return;
            }

            // actual method body here

            switch (args.State)
            {
                case GameState.Init:
                    var rect = new Rect(0, 0, cvs.ActualWidth, cvs.ActualHeight);
                    _game.Initialize(rect);
                    break;
                case GameState.Title:
                    _game.StartGame();
                    break;
                case GameState.Playing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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

            _game.Resize(new Rect(0, 0, cvsSize.Width, cvsSize.Height));
        }

        #region Pan and Zoom

        private bool _inManipulation = false;

        private void OnPointerPressed(object sender, PointerEventArgs args)
        {
            _gestureRecognizer.ProcessDownEvent(args.CurrentPoint);
            args.Handled = true;
        }

        private void OnPointerMoved(object sender, PointerEventArgs args)
        {
            _gestureRecognizer.ProcessMoveEvents(args.GetIntermediatePoints());
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

            _game.ZoomToPoint(coords, wheelTicks * 0.1f);
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
            _game.PanBy(screenDelta);

            _game.ZoomToPoint(args.Position.ToVector2(), args.Delta.Scale - 1.0f);
        }

        private void gestureRecognizer_ManipulationCompleted(GestureRecognizer sender,
            ManipulationCompletedEventArgs args)
        {
            _inManipulation = false;
        }

        #endregion
    }
}
