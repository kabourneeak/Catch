using Catch.Base;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class HexagonGraphics : IGraphicsComponent, IIndicator
    {
        private readonly StyleArgs _style;
        private readonly float _radius;
        private readonly float _radiusH;

        public HexagonGraphics(float radius, StyleArgs style)
        {
            _style = style;
            _radius = radius;
            _radiusH = HexUtils.GetRadiusHeight(_radius);
        }

        public DrawLayer Layer { get; set; }

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

            // define brush
            _brush = _style.CreateBrush(createArgs);
            _brush.Opacity = _style.BrushOpacity;

            // define path
            var pb = new CanvasPathBuilder(createArgs.ResourceCreator);

            pb.BeginFigure(-1 * _radius * HexUtils.COS60, _radiusH);
            pb.AddLine(_radius * HexUtils.COS60, _radiusH);
            pb.AddLine(_radius, 0);
            pb.AddLine(_radius * HexUtils.COS60, -1 * _radiusH);
            pb.AddLine(-1 * _radius * HexUtils.COS60, -1 * _radiusH);
            pb.AddLine(-1 * _radius, 0);
            pb.EndFigure(CanvasFigureLoop.Closed);

            // create and cache
            var geo = CanvasGeometry.CreatePath(pb);

            _geo = CanvasCachedGeometry.CreateStroke(geo, _style.StrokeWidth, _style.StrokeStyle);
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
            drawArgs.Ds.DrawCachedGeometry(_geo, _brush);
        }
    }
}