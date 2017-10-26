using System.Numerics;
using Catch.Graphics;

namespace Catch.Services
{
    public interface IScreenController : IViewportController, IGraphicsResource
    {
        void Initialize(Vector2 size);

        bool AllowPredecessorUpdate();

        bool AllowPredecessorDraw();

        bool AllowPredecessorInput();

        void Update(float deviceTicks);

        void Draw(DrawArgs drawArgs);
    }
}
