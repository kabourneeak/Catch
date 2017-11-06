using System.Numerics;
using Catch.Services;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Graphics
{
    public class BoxSprite : ISprite
    {
        private static readonly string CfgStyleName = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgStyleName));
        private static readonly string CfgHeight = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgHeight));
        private static readonly string CfgWidth = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgWidth));
        private static readonly string CfgFilled = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgFilled));
        private static readonly string CfgRounded = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgRounded));

        private CanvasStrokeStyle _strokeStyle;
        private CanvasCachedGeometry _geo;

        public float Height { get; }
        public float Width { get; }
        public bool Filled { get; }
        public bool Rounded { get; }
        public IStyle Style { get; }

        public BoxSprite(IConfig config, StyleProvider styleProvider)
        {
            Height = config.GetFloat(CfgHeight);
            Width = config.GetFloat(CfgWidth);
            Filled = config.GetBool(CfgFilled, false);
            Rounded = config.GetBool(CfgRounded, false);
            Style = styleProvider.GetStyle(config.GetString(CfgStyleName));
        }

        public bool IsCreated => _geo != null;

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            DestroyResources();

            // create and cache
            if (Rounded)
                _strokeStyle = new CanvasStrokeStyle() { LineJoin = CanvasLineJoin.Round };
            else
                _strokeStyle = new CanvasStrokeStyle();

            var geo = CanvasGeometry.CreateRectangle(resourceCreator, Width / -2.0f, Height / -2.0f, Width, Height);

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
            if (!IsCreated)
                CreateResources(drawArgs.ResourceCreator);

            if (!Style.IsCreated)
                Style.CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawCachedGeometry(_geo, Style.Brush);
        }

        public void Draw(DrawArgs drawArgs, Vector2 offset)
        {
            if (!IsCreated)
                CreateResources(drawArgs.ResourceCreator);

            if (!Style.IsCreated)
                Style.CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawCachedGeometry(_geo, offset, Style.Brush);
        }

        public void Draw(DrawArgs drawArgs, float offsetX, float offsetY)
        {
            if (!IsCreated)
                CreateResources(drawArgs.ResourceCreator);

            if (!Style.IsCreated)
                Style.CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawCachedGeometry(_geo, offsetX, offsetY, Style.Brush);
        }
    }
}
