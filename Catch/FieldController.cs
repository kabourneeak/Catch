using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.System;
using Catch.Base;
using Catch.Graphics;

namespace Catch
{
    /// <summary>
    /// Handles drawing the field of play, i.e., the map and the agents running around on it.
    /// </summary>
    public class FieldController : IGraphics, IViewportController
    {
        private Vector2 _pan;
        private float _zoom;
        private Matrix3x2 _mapTransform;

        private readonly UiStateModel _ui;
        private readonly List<IAgent> _agents;
        private readonly Map.Map _map;

        public FieldController(UiStateModel ui, List<IAgent> agents, Map.Map map)
        {
            _ui = ui;
            _agents = agents;
            _map = map;

            _zoom = 1.0f;
            _pan = Vector2.Zero;
        }

        public void Initialize()
        {
            _zoom = 1.0f;
            _pan.X = (_ui.WindowSize.X - _map.Size.X) / 2.0f;
            _pan.Y = _ui.WindowSize.Y * -1.0f + (_ui.WindowSize.Y - _map.Size.Y) / 2.0f;
        }

        #region IGraphicsComponent Implementation

        public void Update(float ticks)
        {
            foreach (var agent in _agents)
                agent.Update(ticks);
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            foreach (var agent in _agents)
                agent.CreateResources(createArgs);
        }

        public void DestroyResources()
        {
            foreach (var agent in _agents)
                agent.DestroyResources();
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            // apply view matrix
            drawArgs.PushScale(1.0f, -1.0f);
            drawArgs.PushScale(_zoom);
            drawArgs.PushTranslation(_pan);

            // calculate viewport transform, used for zoom/pan
            Matrix3x2.Invert(drawArgs.CurrentTransform, out _mapTransform);

            // have agents draw themselves
            foreach (var agent in _agents)
                agent.Draw(drawArgs, 0.0f);

            // restore view matrix
            drawArgs.Pop();
            drawArgs.Pop();
            drawArgs.Pop();
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(Vector2 panDelta)
        {
            _pan = Vector2.Add(_pan, Vector2.Multiply(panDelta, 1.0f / _zoom));
        }

        public void ZoomToPoint(Vector2 viewCoords, float zoomDelta)
        {
            var newZoom = Math.Max(0.4f, Math.Min(2.0f, _zoom + zoomDelta));

            var zoomCenter = TranslateToFieldCoords(viewCoords);
            _pan = Vector2.Add(_pan, zoomCenter);
            _pan = Vector2.Multiply(_pan, _zoom / newZoom);
            _pan = Vector2.Subtract(_pan, zoomCenter);

            _zoom = newZoom;
        }

        public void Resize(Vector2 size)
        {

        }

        public void Hover(Vector2 viewCoords, VirtualKeyModifiers keyModifiers)
        {
            var fieldCoords = TranslateToFieldCoords(viewCoords);

            _ui.HoverHexCoords = _map.PointToHexCoords(fieldCoords);
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

        public Vector2 TranslateToFieldCoords(Vector2 viewCoords)
        {
            return Vector2.Transform(viewCoords, _mapTransform);
        }
    }
}
