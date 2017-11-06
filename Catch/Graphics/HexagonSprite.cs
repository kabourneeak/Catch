using System.Numerics;
using Catch.Services;
using CatchLibrary.HexGrid;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Graphics
{
    public class HexagonSprite : ISprite
    {
        private static readonly string CfgStyleName = ConfigUtils.GetConfigPath(nameof(HexagonSprite), nameof(CfgStyleName));
        private static readonly string CfgRadius = ConfigUtils.GetConfigPath(nameof(HexagonSprite), nameof(CfgRadius));
        private static readonly string CfgFilled = ConfigUtils.GetConfigPath(nameof(HexagonSprite), nameof(CfgFilled));

        private CanvasStrokeStyle _strokeStyle;
        private CanvasCachedGeometry _geo;

        public float Radius { get; }
        public bool Filled { get; }
        public IStyle Style { get; }

        public HexagonSprite(IConfig config, StyleProvider styleProvider)
        {
            Radius = config.GetFloat(CfgRadius);
            Filled = config.GetBool(CfgFilled, false);
            Style = styleProvider.GetStyle(config.GetString(CfgStyleName));
        }

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            DestroyResources();

            // define path
            var pb = new CanvasPathBuilder(resourceCreator);
            var radiusH = HexUtils.GetRadiusHeight(Radius);

            pb.BeginFigure(-1 * Radius * HexUtils.COS60, radiusH);
            pb.AddLine(Radius * HexUtils.COS60, radiusH);
            pb.AddLine(Radius, 0);
            pb.AddLine(Radius * HexUtils.COS60, -1 * radiusH);
            pb.AddLine(-1 * Radius * HexUtils.COS60, -1 * radiusH);
            pb.AddLine(-1 * Radius, 0);
            pb.EndFigure(CanvasFigureLoop.Closed);

            // create and cache
            _strokeStyle = new CanvasStrokeStyle();

            var geo = CanvasGeometry.CreatePath(pb);

            if (Filled)
                _geo = CanvasCachedGeometry.CreateFill(geo);
            else
                _geo = CanvasCachedGeometry.CreateStroke(geo, Style.StrokeWidth, _strokeStyle);
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

            drawArgs.Ds.DrawCachedGeometry(_geo, Style.Brush);
        }

        public void Draw(DrawArgs drawArgs, Vector2 offset)
        {
            if (_geo == null)
                CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawCachedGeometry(_geo, offset, Style.Brush);
        }

        public void Draw(DrawArgs drawArgs, float offsetX, float offsetY)
        {
            if (_geo == null)
                CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawCachedGeometry(_geo, offsetX, offsetY, Style.Brush);
        }
    }
}
