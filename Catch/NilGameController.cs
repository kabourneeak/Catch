using System.Numerics;
using Catch.Base;

namespace Catch
{
    public class NilGameController : IGameController
    {
        #region IGameController Implementation

        public event GameStateChangedHandler GameStateChangeRequested;

        public void Initialize(Vector2 size)
        {
            // do nothing
        }

        #endregion

        #region IGraphicsComponent Implementation

        public void Update(float ticks)
        {
            // do nothing
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            // do nothing
        }

        public void DestroyResources()
        {
            // do nothing
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            // do nothing
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(Vector2 panDelta)
        {
            // do nothing
        }

        public void ZoomToPoint(Vector2 viewCoords, float zoomDelta)
        {
            // do nothing
        }

        public void Resize(Vector2 size)
        {
            // do nothing
        }

        #endregion
    }
}
