using System.Numerics;
using Catch.Base;
using Catch.Graphics;

namespace Catch.Services
{
    public interface IScreenController : IViewportController, IGraphicsResource, IUpdatable, IDrawable
    {
        void Initialize(Vector2 size);

        bool AllowPredecessorUpdate();

        bool AllowPredecessorDraw();

        bool AllowPredecessorInput();
    }
}
