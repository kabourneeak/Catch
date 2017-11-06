using System.Numerics;
using Catch.Graphics;
using Catch.Services;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Towers
{
    public class GunTowerBodySprite : ISprite
    {
        private static readonly string CfgStyleName = ConfigUtils.GetConfigPath(nameof(GunTowerBodySprite), nameof(CfgStyleName));

        private CanvasStrokeStyle _strokeStyle;
        private CanvasCachedGeometry _geo;

        public IStyle Style { get; }

        public GunTowerBodySprite(IConfig config, StyleProvider styleProvider)
        {
            Style = styleProvider.GetStyle(config.GetString(CfgStyleName));
        }

        public bool IsCreated => _geo != null;

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            DestroyResources();

            // create stroke
            _strokeStyle = new CanvasStrokeStyle();

            // create geometry
            var body = CanvasGeometry.CreateCircle(resourceCreator, new Vector2(0.0f), 24);
            var cannon = CanvasGeometry.CreateRectangle(resourceCreator, 23, -3, 10, 6);

            var comb = body.CombineWith(cannon, Matrix3x2.Identity, CanvasGeometryCombine.Union);

            // cache
            _geo = CanvasCachedGeometry.CreateStroke(comb, Style.StrokeWidth, _strokeStyle);
        }

        public void DestroyResources()
        {
            if (_geo == null)
                return;

            _geo.Dispose();
            _geo = null;

            _strokeStyle.Dispose();
            _strokeStyle = null;
        }

        public void Draw(DrawArgs drawArgs)
        {
            if (_geo == null)
                CreateResources(drawArgs.ResourceCreator);

            if (!Style.IsCreated)
                Style.CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawCachedGeometry(_geo, Style.Brush);
        }

        public void Draw(DrawArgs drawArgs, Vector2 offset)
        {
            if (_geo == null)
                CreateResources(drawArgs.ResourceCreator);

            if (!Style.IsCreated)
                Style.CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawCachedGeometry(_geo, offset, Style.Brush);
        }

        public void Draw(DrawArgs drawArgs, float offsetX, float offsetY)
        {
            if (_geo == null)
                CreateResources(drawArgs.ResourceCreator);

            if (!Style.IsCreated)
                Style.CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawCachedGeometry(_geo, offsetX, offsetY, Style.Brush);
        }
    }
}
