using System;
using System.Linq;
using System.Numerics;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;

namespace Catch.Level
{
    /// <summary>
    /// Handles drawing the field of play, i.e., the map and the agents running around on it.
    /// </summary>
    public class FieldController : IViewportController
    {
        private static readonly DrawLayer[] DrawLayers = EnumUtils.GetEnumAsList<DrawLayer>().ToArray();

        private readonly UiStateModel _uiState;
        private readonly MapModel _map;
        private readonly IIndicatorRegistry _indicatorRegistry;
        private readonly float _tileRadius;

        private Vector2 _pan;
        private float _zoom;
        private Vector2 _bottomLeftViewLimit;
        private Vector2 _topRightViewLimit;
        private Matrix3x2 _mapTransform;

        public FieldController(IConfig config, UiStateModel uiState, MapModel map, IIndicatorRegistry indicatorRegistry)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _indicatorRegistry = indicatorRegistry ?? throw new ArgumentNullException(nameof(indicatorRegistry));
            _uiState = uiState ?? throw new ArgumentNullException(nameof(uiState));
            _map = map ?? throw new ArgumentNullException(nameof(map));

            _tileRadius = config.GetFloat(CoreConfig.TileRadius);

            _pan = Vector2.Zero;
            _zoom = 1.0f;
        }

        public void Initialize()
        {
            _zoom = 1.0f;
            _pan.X = (_uiState.WindowSize.X - _map.Size.X) / 2.0f;
            _pan.Y = _uiState.WindowSize.Y * -1.0f + (_uiState.WindowSize.Y - _map.Size.Y) / 2.0f;

            _bottomLeftViewLimit = new Vector2(_tileRadius / 2 * -1);
            _topRightViewLimit = Vector2.Add(_uiState.WindowSize, new Vector2(_tileRadius / 2));
        }

        public void Update(float deviceTicks)
        {
            // do nothing
        }

        #region IDrawable Implementation

        public void Draw(DrawArgs drawArgs)
        {
            // apply view matrix
            drawArgs.PushScale(1.0f, -1.0f);
            drawArgs.PushScale(_zoom);
            drawArgs.PushTranslation(_pan);

            // apply level of detail
            drawArgs.LevelOfDetail = _zoom > 0.5 ? DrawLevelOfDetail.Normal : DrawLevelOfDetail.Low;

            // calculate viewport transform, used for zoom/pan
            Matrix3x2.Invert(drawArgs.CurrentTransform, out _mapTransform);

            // calculate visible field coords
            var bottomLeftFieldCoords = TranslateToFieldCoords(_bottomLeftViewLimit);
            var topRightFieldCoords = TranslateToFieldCoords(_topRightViewLimit);

            // find indicators which are currently on screen
            var culledIndicators = _indicatorRegistry.Indicators
                .Where(tm => bottomLeftFieldCoords.X <= tm.Position.X && topRightFieldCoords.X >= tm.Position.X)
                .ToArray();

            // have agents draw themselves layer by layer
            foreach (var drawLayer in DrawLayers)
            {
                drawArgs.Layer = drawLayer;

                for (var i = 0; i < culledIndicators.Length; ++i)
                {
                    if (culledIndicators[i].Layer == drawArgs.Layer
                        && culledIndicators[i].LevelOfDetail.HasFlag(drawArgs.LevelOfDetail))
                    {
                        culledIndicators[i].Draw(drawArgs);
                    }
                }
            }

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
            _bottomLeftViewLimit = new Vector2(_tileRadius / 2 * -1);
            _topRightViewLimit = Vector2.Add(size, new Vector2(_tileRadius / 2));
        }

        public void Hover(HoverEventArgs eventArgs)
        {
            var fieldCoords = TranslateToFieldCoords(eventArgs.ViewCoords);

            var hoverCoords = _map.PointToHexCoords(fieldCoords);
            _uiState.HoverHexCoords = hoverCoords;
            _uiState.HoverTile = _map.HasHex(hoverCoords) ? _map.GetTileModel(hoverCoords) : null;
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

        private Vector2 TranslateToFieldCoords(Vector2 viewCoords)
        {
            return Vector2.Transform(viewCoords, _mapTransform);
        }
    }
}
