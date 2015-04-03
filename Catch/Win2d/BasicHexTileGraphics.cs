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

        public BasicHexTileGraphics(IHexTile tile, float radius)
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

        public void CreateResources(DrawArgs drawArgs)
        {
            // do nothing
        }

        public void Draw(DrawArgs drawArgs)
        {
            // TODO cache this between multiple instances

            var pb = new CanvasPathBuilder(drawArgs.Ds);
            pb.BeginFigure(-1 * _radius * HexUtils.COS60, _radiusH);
            pb.AddLine(_radius * HexUtils.COS60, _radiusH);
            pb.AddLine(_radius, 0);
            pb.AddLine(_radius * HexUtils.COS60, -1 * _radiusH);
            pb.AddLine(-1 * _radius * HexUtils.COS60, -1 * _radiusH);
            pb.AddLine(-1 * _radius, 0);
            pb.EndFigure(CanvasFigureLoop.Closed);

            var geo = CanvasGeometry.CreatePath(pb);

            drawArgs.Ds.DrawGeometry(geo, Position.X, Position.Y, Colour, 4);
        }
    }
}
