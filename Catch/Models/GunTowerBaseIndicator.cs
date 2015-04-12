using System.Numerics;
using Windows.UI;
using Catch.Base;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class GunTowerBaseIndicator : IIndicator
    {
        private readonly GunTower _tower;

        public GunTowerBaseIndicator(GunTower tower, IConfig config)
        {
            _tower = tower;
            Layer = DrawLayer.Tower;
        }

        public void Update(float ticks)
        {
            // do nothing
        }

        private static int _createFrameId = -1;
        private static CanvasCachedGeometry _geo;
        private static ICanvasBrush _brush;

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            if (!(createArgs.IsMandatory || _geo == null))
                return;

            if (_createFrameId == createArgs.FrameId)
                return;

            _createFrameId = createArgs.FrameId;

            if (_geo != null)
                _geo.Dispose();

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

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.PushRotation(_tower.Rotation);

            drawArgs.Ds.DrawCachedGeometry(_geo, _brush);

            drawArgs.Pop();
        }

        public DrawLayer Layer { get; set; }
    }
}