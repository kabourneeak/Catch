using System.Numerics;
using Catch.Base;
using Catch.Graphics;

namespace Catch
{
    public delegate void GameStateChangedHandler(object sender, GameStateArgs e);

    public interface IGameController : IViewportController, IGraphics, IUpdatable
    {
        event GameStateChangedHandler GameStateChangeRequested;

        void Initialize(Vector2 size, GameStateArgs args);
    }
}
