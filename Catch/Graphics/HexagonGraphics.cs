using CatchLibrary.HexGrid;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Graphics
{
    public class HexagonGraphics : IGraphicsResource, IDrawable
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

        private int _createFrameId = -1;
        private CanvasCachedGeometry _geo;
        private ICanvasBrush _brush;

        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || _geo == null))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            _createFrameId = args.FrameId;

            // define brush
            _brush = _style.CreateBrush(args);
            _brush.Opacity = _style.BrushOpacity;

            // define path
            var pb = new CanvasPathBuilder(args.ResourceCreator);

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