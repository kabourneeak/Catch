using System.Numerics;
using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Towers
{
    public class GunTowerBaseIndicator : IIndicator
    {
        public GunTowerBaseIndicator(IConfig config)
        {

        }

        public void Update(float ticks)
        {
            // do nothing
        }

        private int _createFrameId = -1;
        private CanvasCachedGeometry _geo;
        private ICanvasBrush _brush;

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            if (!(createArgs.IsMandatory || _geo == null))
                return;

            if (_createFrameId == createArgs.FrameId)
                return;

            DestroyResources();

            _createFrameId = createArgs.FrameId;

            // define style
            var strokeStyle = new CanvasStrokeStyle() { };
            var strokeWidth = 4;

            // define brush
            _brush = new CanvasSolidColorBrush(createArgs.ResourceCreator, Colors.RoyalBlue);

            // create geometry
            var body = CanvasGeometry.CreateCircle(createArgs.ResourceCreator, new Vector2(0.0f), 24);
            var cannon = CanvasGeometry.CreateRectangle(createArgs.ResourceCreator, 23, -3, 10, 6);

            var comb = body.CombineWith(cannon, Matrix3x2.Identity, CanvasGeometryCombine.Union);

            // cache
            _geo = CanvasCachedGeometry.CreateStroke(comb, strokeWidth, strokeStyle);
        }

        public void DestroyResources()
        {
            if (_geo == null)
                return;

            _geo.Dispose();
            _geo = null;

            _createFrameId = -1;
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            drawArgs.PushRotation(rotation);

            drawArgs.Ds.DrawCachedGeometry(_geo, _brush);

            drawArgs.Pop();
        }

        public DrawLayer Layer => DrawLayer.Tower;
    }
}