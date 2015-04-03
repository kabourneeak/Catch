using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Catch.Base
{
    /// <summary>
    /// Just the basic methods for drawing
    /// </summary>
    public interface IGraphicsComponent
    {
        // Events
        void Update(float ticks);

        void CreateResources(DrawArgs drawArgs);

        void Draw(DrawArgs drawArgs);
    }

}