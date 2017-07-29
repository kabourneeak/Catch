using Catch.Base;
using CatchLibrary.HexGrid;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Graphics
{
    public class HexagonIndicator : IIndicator, IGraphicsResource
    {
        public StyleArgs Style { get; protected set; }
        public float Radius { get; protected set; }
        public bool Filled { get; protected set; }

        public HexagonIndicator()
        {

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
            _brush = Style.CreateBrush(args);
            _brush.Opacity = Style.BrushOpacity;

            // define path
            var pb = new CanvasPathBuilder(args.ResourceCreator);
            var radiusH = HexUtils.GetRadiusHeight(Radius);

            pb.BeginFigure(-1 * Radius * HexUtils.COS60, radiusH);
            pb.AddLine(Radius * HexUtils.COS60, radiusH);
            pb.AddLine(Radius, 0);
            pb.AddLine(Radius * HexUtils.COS60, -1 * radiusH);
            pb.AddLine(-1 * Radius * HexUtils.COS60, -1 * radiusH);
            pb.AddLine(-1 * Radius, 0);
            pb.EndFigure(CanvasFigureLoop.Closed);

            // create and cache
            var geo = CanvasGeometry.CreatePath(pb);

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

        public DrawLayer Layer { get; protected set; }

        public DrawLevelOfDetail LevelOfDetail { get; protected set; } = DrawLevelOfDetail.All;

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, _brush);
        }
    }
}