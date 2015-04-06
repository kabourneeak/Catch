using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Catch.Base
{
    /// <summary>
    /// Just the basic methods for drawing and animation
    /// </summary>
    public interface IGraphicsComponent
    {
        // Events
        void Update(float ticks);

        void CreateResources(CreateResourcesArgs createArgs);

        void Draw(DrawArgs drawArgs);
    }
}