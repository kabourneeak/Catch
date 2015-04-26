using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Catch.Win2d;
using Microsoft.Graphics.Canvas;
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

            cvs.Input.PointerPressed += cvs_PointerPressed;
            cvs.Input.PointerWheelChanged += cvs_PointerWheelChanged;
            cvs.Input.PointerMoved += cvs_PointerMoved;
        }

        #region " Game Loop "

        private void cvs_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            _game.CreateResources(sender.Device);
        }

        private void cvs_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            // TODO make the number of ticks depend on wall time elapsed, maybe some other gamespeed setting?
            _game.Update(1);
        }

        private void cvs_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            _game.Draw(args.DrawingSession);
        }

        #endregion

        private void GameStateHandler(object sender, GameStateChangedEventArgs e)
        {
            
            if (!Dispatcher.HasThreadAccess)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate { GameStateHandler(sender, e); }).Forget();
                return;
            }

            // actual method body here

            switch (e.State)
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

        private void DisplayLog(String msg)
        {
            if (!Dispatcher.HasThreadAccess)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate { DisplayLog(msg); }).Forget();
                return;
            }

            txtLog.Text += ("\n" + msg);

            scrlLog.ChangeView(0.0, scrlLog.ScrollableHeight, 1.0f);
        }

        private void Canvas_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            //
            // update layout
            //

            // animated area
            cvs.Height = e.NewSize.Height;
            cvs.Width = e.NewSize.Width;

            // log
            Canvas.SetLeft(scrlLog, layout.ActualWidth - scrlLog.ActualWidth);
        }


        private void cvs_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            var cvsSize = cvs.Size;

            _game.Resize(new Rect(0, 0, cvsSize.Width, cvsSize.Height));
        }


        #region Pan and Zoom

        private Vector2 _lastPoint = Vector2.Zero;
        private Vector2 _screenDelta = Vector2.Zero;

        private void cvs_PointerPressed(object sender, PointerEventArgs e)
        {
            var coords = e.CurrentPoint.Position.ToVector2();

            var translated = _game.TranslateToMap(new Vector2((float) coords.X, (float) coords.Y));

            DisplayLog(string.Format("PointerPresses: Screen:({0:F0},{1:F0}) Translated:({2:F0},{3:F0})", coords.X, coords.Y, translated.X, translated.Y));

            _lastPoint = coords;
        }

        private void cvs_PointerMoved(object sender, PointerEventArgs e)
        {
            var coords = e.CurrentPoint.Position.ToVector2();

            if (e.CurrentPoint.Properties.IsLeftButtonPressed)
            {
                _screenDelta.Y = _lastPoint.Y - coords.Y;
                _screenDelta.X = coords.X - _lastPoint.X;
                var worldDelta = Vector2.Multiply(_screenDelta, 1 / _game.Zoom);

                _game.PanBy(worldDelta);
            }

            _lastPoint = coords;
        }

        private void cvs_PointerReleased(object sender, PointerEventArgs e)
        {
           
        }

        private void cvs_PointerWheelChanged(object sender, PointerEventArgs args)
        {
            var delta = args.CurrentPoint.Properties.MouseWheelDelta;
            delta = delta / 120;

            _game.Zoom = _game.Zoom + (delta * 0.1f);
        }

        #endregion

        private void scrlLog_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void scrlLog_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void scrlLog_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void scrlLog_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void txtLog_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void txtLog_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void txtLog_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void txtLog_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }
    }
}
