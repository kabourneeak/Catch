using System.Collections.Generic;
using System.Numerics;
using Windows.System;
using Catch.Base;
using Catch.Graphics;

namespace Catch
{
    /// <summary>
    /// Handles drawing and event handling for the player control overlay, i.e., the buttons
    /// and things the user will interact with
    /// </summary>
    public class OverlayController : IGraphicsComponent, IViewportController
    {
        private readonly UiStateModel _ui;
        private readonly List<IAgent> _agents;
        private readonly Map.Map _map;

        public OverlayController(UiStateModel ui, List<IAgent> agents, Map.Map map)
        {
            _ui = ui;
            _agents = agents;
            _map = map;
        }

        public void Initialize()
        {

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

        public void Touch(Vector2 viewCoords, VirtualKeyModifiers keyModifiers)
        {
            // TODO
        }

        public void KeyPress(VirtualKey key)
        {
            // TODO
        }

        #endregion
    }
}
