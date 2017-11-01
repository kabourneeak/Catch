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
        private readonly IStyle _style;

        public GunTowerBaseIndicator(IConfig config, IStyle style)
        {
            _style = style;
        }

        private int _createFrameId = -1;
        private CanvasCachedGeometry _geo;

        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || _geo == null))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            _createFrameId = args.FrameId;

            // define style
            var strokeStyle = new CanvasStrokeStyle() { };
            var strokeWidth = 4;

            // create geometry
            var body = CanvasGeometry.CreateCircle(args.ResourceCreator, new Vector2(0.0f), 24);
            var cannon = CanvasGeometry.CreateRectangle(args.ResourceCreator, 23, -3, 10, 6);

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

            drawArgs.Ds.DrawCachedGeometry(_geo, _style.Brush);

            drawArgs.Pop();
        }

        public DrawLayer Layer => DrawLayer.Tower;

        public DrawLevelOfDetail LevelOfDetail => DrawLevelOfDetail.NormalHigh;

    }
}