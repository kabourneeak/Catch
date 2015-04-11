using System.Numerics;
using Windows.UI;
using Catch.Base;
using Microsoft.Graphics.Canvas;

namespace Catch.Win2d
{
    public class BasicHexTileGraphics : IGraphicsComponent
    {
        private readonly float _radius;
        private readonly float _radiusH;

        public Color Colour { get; set; }
        public float Radius { get { return _radius;} }

        public BasicHexTileGraphics(Tile tile, float radius)
        {
            _radius = radius;
            _radiusH = HexUtils.GetRadiusHeight(_radius);

            var col = tile.Column;
            var row = tile.Row;

            var x = _radius + (col * (_radius + _radius * HexUtils.COS60));
            var y = (col % 2 * _radiusH) + (row * 2 * _radiusH) + _radiusH;

            Position = new Vector2(x, y);

            Colour = Colors.Red;
        }

        public Vector2 Position { get; private set; }
        public DrawLayer Layer { get; private set; }

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
            var strokeStyle = new CanvasStrokeStyle() {};
            var strokeWidth = 4;

            // define brush
            _brush = new CanvasSolidColorBrush(createArgs.ResourceCreator, Colour);
   
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
            
            _geo = CanvasCachedGeometry.CreateStroke(geo, strokeWidth, strokeStyle);
        }

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, Position, _brush);
        }
    }
}
