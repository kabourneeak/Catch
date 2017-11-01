using System.Numerics;
using Catch.Graphics;
using Catch.Services;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Towers
{
    public class GunTowerBodySprite : ISprite
    {
        private static readonly string CfgStyleName = ConfigUtils.GetConfigPath(nameof(GunTowerBodySprite), nameof(CfgStyleName));

        private int _createFrameId = -1;
        private CanvasStrokeStyle _strokeStyle;
        private CanvasCachedGeometry _geo;

        public IStyle Style { get; }

        public GunTowerBodySprite(IConfig config, StyleProvider styleProvider)
        {
            Style = styleProvider.GetStyle(config.GetString(CfgStyleName));
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || _geo == null))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            _createFrameId = args.FrameId;

            // create stroke
            _strokeStyle = new CanvasStrokeStyle();

            // create geometry
            var body = CanvasGeometry.CreateCircle(args.ResourceCreator, new Vector2(0.0f), 24);
            var cannon = CanvasGeometry.CreateRectangle(args.ResourceCreator, 23, -3, 10, 6);

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

            _createFrameId = -1;
        }

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, Style.Brush);
        }

        public void Draw(DrawArgs drawArgs, Vector2 offset)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, offset, Style.Brush);
        }

        public void Draw(DrawArgs drawArgs, float offsetX, float offsetY)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, offsetX, offsetY, Style.Brush);
        }
    }
}
