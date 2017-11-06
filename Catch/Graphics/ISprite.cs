using System;
using System.Numerics;

namespace Catch.Graphics
{
    public interface ISprite : IGraphicsResource
    {
        void Draw(DrawArgs drawArgs);

        void Draw(DrawArgs drawArgs, Vector2 offset);

        [Obsolete]
        void Draw(DrawArgs drawArgs, float offsetX, float offsetY);
    }
}
