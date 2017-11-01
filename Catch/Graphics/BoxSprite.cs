using System.Numerics;
using Catch.Services;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Graphics
{
    public class BoxSprite : ISprite
    {
        private static readonly string CfgStyleName = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgStyleName));
        private static readonly string CfgHeight = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgHeight));
        private static readonly string CfgWidth = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgWidth));
        private static readonly string CfgFilled = ConfigUtils.GetConfigPath(nameof(BoxSprite), nameof(CfgFilled));

        private int _createFrameId = -1;
        private CanvasCachedGeometry _geo;

        public float Height { get; }
        public float Width { get; }
        public bool Filled { get; }
        public IStyle Style { get; }

        public BoxSprite(IConfig config, StyleProvider styleProvider)
        {
            Height = config.GetFloat(CfgHeight);
            Width = config.GetFloat(CfgWidth);
            Filled = config.GetBool(CfgFilled, false);
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

            // create and cache
            var geo = CanvasGeometry.CreateRectangle(args.ResourceCreator, Width / -2.0f, Height / -2.0f, Width, Height);

            if (Filled)
                _geo = CanvasCachedGeometry.CreateFill(geo);
            else
                _geo = CanvasCachedGeometry.CreateStroke(geo, Style.StrokeWidth, Style.StrokeStyle);
        }

        public void DestroyResources()
        {
            if (_geo == null)
                return;

            _geo.Dispose();
            _geo = null;

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
