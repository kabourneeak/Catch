using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class Hexagon : IDrawable
    {
        public Point CenterPoint { get; set; }
        public Color Colour { get; set; }
        public int Radius { get; set; }

        public Hexagon()
        {
            CenterPoint = new Point(0, 0);
            Colour = Colors.Red;
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
