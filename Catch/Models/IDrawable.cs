using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public interface IDrawable
    {
        void Draw(CanvasDrawingSession drawingSession);

        void Update();
    }
}
