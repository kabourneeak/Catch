using System.Numerics;
using Windows.System;
using Catch.Graphics;

namespace Catch
{
    /// <summary>
    /// Handles drawing and event handling for the player control overlay, i.e., the buttons
    /// and things the user will interact with
    /// </summary>
    public class OverlayController : IGraphicsComponent, IViewportController
    {
        private Vector2 WindowSize { get; set; }

        public OverlayController()
        {

        }

        public void Initialize(Vector2 size)
        {
            WindowSize = size;
        }

        #region IGraphicsComponent Implementation

        public void Update(float ticks)
        {

        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {

        }

        public void DestroyResources()
        {

        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {

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

        public void Hover(Vector2 viewCoords, VirtualKeyModifiers keyModifiers)
        {
            // TODO
        }

        #endregion
    }
}
