using System;
using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Graphics;

namespace Catch
{
    /// <summary>
    /// Handles drawing the field of play, i.e., the map and the agents running around on it.
    /// </summary>
    public class FieldController : IGraphicsComponent, IViewportController
    {
        private Vector2 WindowSize { get; set; }
        public float Zoom { get; private set; }
        public Vector2 Pan => _pan;

        private Vector2 _pan;
        private Matrix3x2 _mapTransform;

        private readonly List<IAgent> _agents;
        private readonly Map.Map _map;

        public FieldController(List<IAgent> agents, Map.Map map)
        {
            _agents = agents;
            _map = map;

            Zoom = 1.0f;
            _pan = Vector2.Zero;
        }

        public void Initialize(Vector2 size)
        {
            WindowSize = size;

            Zoom = 1.0f;
            _pan.X = (WindowSize.X - _map.Size.X) / 2.0f;
            _pan.Y = WindowSize.Y * -1.0f + (WindowSize.Y - _map.Size.Y) / 2.0f;
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
            // apply view matrix
            drawArgs.PushScale(1.0f, -1.0f);
            drawArgs.PushScale(Zoom);
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
            _pan = Vector2.Add(_pan, Vector2.Multiply(panDelta, 1.0f / Zoom));
        }

        public void ZoomToPoint(Vector2 viewCoords, float zoomDelta)
        {
            var newZoom = Math.Max(0.4f, Math.Min(2.0f, Zoom + zoomDelta));

            var zoomCenter = TranslateToMap(viewCoords);
            _pan = Vector2.Add(_pan, zoomCenter);
            _pan = Vector2.Multiply(_pan, Zoom / newZoom);
            _pan = Vector2.Subtract(_pan, zoomCenter);

            Zoom = newZoom;
        }

        public void Resize(Vector2 size)
        {
            WindowSize = size;
        }

        #endregion

        public Vector2 TranslateToMap(Vector2 viewCoords)
        {
            return Vector2.Transform(viewCoords, _mapTransform);
        }
    }
}
