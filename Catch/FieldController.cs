using System;
using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

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

        private readonly ILevelStateModel _level;
        private readonly List<IAgent> _agents;

        public FieldController(ILevelStateModel level, List<IAgent> agents)
        {
            _level = level;
            _agents = agents;

            _zoom = 1.0f;
            _pan = Vector2.Zero;
        }

        public void Initialize()
        {
            _zoom = 1.0f;
            _pan.X = (_level.Ui.WindowSize.X - _level.Map.Size.X) / 2.0f;
            _pan.Y = _level.Ui.WindowSize.Y * -1.0f + (_level.Ui.WindowSize.Y - _level.Map.Size.Y) / 2.0f;
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

        public void PanBy(PanByEventArgs eventArgs)
        {
            _pan = Vector2.Add(_pan, Vector2.Multiply(eventArgs.PanDelta, 1.0f / _zoom));
        }

        public void ZoomToPoint(ZoomToPointEventArgs eventArgs)
        {
            var newZoom = Math.Max(0.4f, Math.Min(2.0f, _zoom + eventArgs.ZoomDelta));

            var zoomCenter = TranslateToFieldCoords(eventArgs.ViewCoords);
            _pan = Vector2.Add(_pan, zoomCenter);
            _pan = Vector2.Multiply(_pan, _zoom / newZoom);
            _pan = Vector2.Subtract(_pan, zoomCenter);

            _zoom = newZoom;
        }

        public void Resize(Vector2 size)
        {

        }

        public void Hover(HoverEventArgs eventArgs)
        {
            var fieldCoords = TranslateToFieldCoords(eventArgs.ViewCoords);

            _level.Ui.HoverHexCoords = _level.Map.PointToHexCoords(fieldCoords);
        }

        public void Touch(TouchEventArgs eventArgs)
        {
            // TODO
        }

        public void KeyPress(KeyPressEventArgs eventArgs)
        {
            // do nothing
            // the overlay is responsible for handling key presses
        }

        #endregion

        public Vector2 TranslateToFieldCoords(Vector2 viewCoords)
        {
            return Vector2.Transform(viewCoords, _mapTransform);
        }
    }
}
