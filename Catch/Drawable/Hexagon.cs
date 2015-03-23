using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Catch.Base;
using Catch.Models;
using Microsoft.Graphics.Canvas;

namespace Catch.Drawable
{
    public class Hexagon : BasicHexTile, IHexTile, IDrawable
    {
        public class ConfigKeys
        {
            public static readonly string Radius = typeof(Hexagon).FullName + "Radius";
        }

        public Vector2 CenterPoint { get; set; }
        public Color Colour { get; set; }
        public int Radius { get; set; }

        public const float SIN60 = 0.8660254f;
        public const float COS60 = 0.5f;

        public Hexagon(int row, int col) : base(row, col)
        {
            Radius = 60;
            var radiusH = (float)(Radius * Hexagon.SIN60);

            //var x = (float)((col * Radius * 3) + Radius + (row % 2 * Radius * 1.5));
            //var y = (float)((row * radiusH) + radiusH);

            var x = (float) (Radius + (col * (Radius + Radius * COS60)));
            var y = (float) ((col % 2 * radiusH) + (row * 2 * radiusH) + radiusH);

            CenterPoint = new Vector2(x, y);

            Colour = Colors.Red;
        }

        public void Draw(CanvasDrawingSession ds)
        {
            // TODO cache this between multiple instances
            var pb = new CanvasPathBuilder(ds);
            pb.BeginFigure(-1 * Radius * COS60, Radius * SIN60);
            pb.AddLine(Radius * COS60, Radius * SIN60);
            pb.AddLine(Radius, 0);
            pb.AddLine(Radius * COS60, -1 * Radius * SIN60);
            pb.AddLine(-1 * Radius * COS60, -1 * Radius * SIN60);
            pb.AddLine(-1 * Radius, 0);
            pb.EndFigure(CanvasFigureLoop.Closed);

            var geo = CanvasGeometry.CreatePath(pb);

            ds.DrawGeometry(geo, (float) CenterPoint.X, (float) CenterPoint.Y, Colour, 4);
        }
    }
}
