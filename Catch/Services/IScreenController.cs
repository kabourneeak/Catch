using System.Numerics;
using Catch.Base;
using Catch.Graphics;

namespace Catch.Services
{
    public interface IScreenController : IViewportController, IGraphics, IUpdatable
    {
        void Initialize(Vector2 size, GameStateArgs args);

        bool AllowParentUpdate();

        bool AllowParentDraw();

        bool AllowParentInput();
    }
}
