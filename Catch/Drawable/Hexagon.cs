using System.Numerics;
using Windows.UI;
using Catch.Base;
using Catch.Models;
using Microsoft.Graphics.Canvas;

namespace Catch.Drawable
{
    public class Hexagon : BasicHexTile, IHexTile, IDrawable
    {
        private readonly float _radius;
        private readonly float _radiusH;

        public class ConfigKeys
        {
        }

        public Vector2 CenterPoint { get; set; }
        public Color Colour { get; set; }
        public float Radius { get { return _radius;} }

        public const float SIN60 = 0.8660254f;
        public const float COS60 = 0.5f;

        public Hexagon(int row, int col, float radius) : base(row, col)
        {
            _radius = radius;
            _radiusH = GetRadiusHeight(_radius);

            var x = _radius + (col * (_radius + _radius * COS60));
            var y = (col % 2 * _radiusH) + (row * 2 * _radiusH) + _radiusH;

            CenterPoint = new Vector2(x, y);

            Colour = Colors.Red;
        }

        public void Draw(CanvasDrawingSession ds)
        {
            // TODO cache this between multiple instances

            var pb = new CanvasPathBuilder(ds);
            pb.BeginFigure(-1 * _radius * COS60, _radiusH);
            pb.AddLine(_radius * COS60, _radiusH);
            pb.AddLine(_radius, 0);
            pb.AddLine(_radius * COS60, -1 * _radiusH);
            pb.AddLine(-1 * _radius * COS60, -1 * _radiusH);
            pb.AddLine(-1 * _radius, 0);
            pb.EndFigure(CanvasFigureLoop.Closed);

            var geo = CanvasGeometry.CreatePath(pb);

            ds.DrawGeometry(geo, CenterPoint.X, CenterPoint.Y, Colour, 4);
        }

        public static float GetRadiusHeight(float radius)
        {
            return radius * SIN60;
        }
    }
}
