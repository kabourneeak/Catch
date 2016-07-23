using System.Numerics;
using Catch.Graphics;

namespace Catch
{
    public delegate void GameStateChangedHandler(object sender, GameStateChangeRequestEventArgs e);

    public interface IGameController : IViewportController, IGraphicsComponent
    {
        event GameStateChangedHandler GameStateChangeRequested;

        void Initialize(Vector2 size);
    }
}
