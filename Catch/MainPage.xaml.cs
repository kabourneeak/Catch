using System;
using System.Collections.Generic;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Catch.Models;
using Microsoft.Graphics.Canvas;

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

        private void cvs_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {

        }

        private void cvs_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            _game.Update();
        }

        private void cvs_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            _game.Draw(args.DrawingSession);

            if (points.Count == 2)
            {

                var dist = Math.Sqrt(((points[0].X - points[1].X) * (points[0].X - points[1].X)) + ((points[0].Y - points[1].Y) * (points[0].Y - points[1].Y)));
                txtLen.Text = String.Format("{0:0.00}", dist);

                args.DrawingSession.DrawLine(points[0].X, points[0].Y, points[1].X, points[1].Y, Colors.Red);

            }
        }

        private void GameStateHandler(object sender, GameStateChangedEventArgs e)
        {
            switch (e.State)
            {
                case GameState.Init:
                    var rect = new Rect(0, 0, cvs.ActualWidth, cvs.ActualHeight);
                    _game.Initialize(rect);
                    break;
                default:
                    throw new ArgumentException("Unhandled game state");
            }
        }

        private void displayLog(String msg)
        {
            txtLog.Text += ("\n" + msg);

            scrlLog.ChangeView(0.0, scrlLog.ScrollableHeight, 1.0f);
        }

        List<PointerData> points = new List<PointerData>(2);

        private void cvs_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var p = e.Pointer.PointerId;

            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
            {
                displayLog(String.Format("[PointerPressed] id:{0}", p));

                if (points.Count < 2)
                {
                    var pd = new PointerData(e.Pointer.PointerId, e.GetCurrentPoint(cvs).Position);
                    points.Add(pd);
                }
                else
                {
                    displayLog("Extra touch ignored");
                }
            }
        }

        private void cvs_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pos = e.GetCurrentPoint(cvs).Position;
            var pid = e.Pointer.PointerId;

            foreach(var pd in points) {
                if (pd.PointerId == pid) {
                    pd.update(pos);
                    break;
                }
            }
        }

        private void cvs_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var p = e.Pointer;

            if (p.PointerDeviceType == PointerDeviceType.Touch)
            {
                displayLog(String.Format("[PointerReleased] id:{0}", p.PointerId));

                var pid = e.Pointer.PointerId;
                var removed = points.RemoveAll((PointerData pd) => {return pd.PointerId == pid;});

                if (removed == 0)
                {
                    displayLog("Unknown touch released");
                }

            }
        }

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
