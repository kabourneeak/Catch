﻿using System;
using System.Linq;
using System.Numerics;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using CatchLibrary.HexGrid;

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
        private readonly PrerenderProvider _prerenderProvider;
        private readonly IIndicatorRegistry _indicatorRegistry;
        private readonly float _tileRadius;
        private readonly float _tileRadiusH;

        private Vector2 _pan;
        private float _zoom;
        private Vector2 _bottomLeftViewLimit;
        private Vector2 _topRightViewLimit;
        private Matrix3x2 _mapTransform;

        public FieldController(IConfig config, UiStateModel uiState, MapModel map, IIndicatorRegistry indicatorRegistry, PrerenderProvider prerenderProvider)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _uiState = uiState ?? throw new ArgumentNullException(nameof(uiState));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _prerenderProvider = prerenderProvider ?? throw new ArgumentNullException(nameof(config));
            _indicatorRegistry = indicatorRegistry ?? throw new ArgumentNullException(nameof(indicatorRegistry));

            _tileRadius = config.GetFloat(CoreConfig.TileRadius);
            _tileRadiusH = HexUtils.GetRadiusHeight(_tileRadius);

            _pan = Vector2.Zero;
            _zoom = 1.0f;
        }

        public void Initialize()
        {
            _zoom = 1.0f;
            _pan.X = (_uiState.WindowSize.X - _map.Size.X) / 2.0f;
            _pan.Y = (_uiState.WindowSize.Y - _map.Size.Y) / 2.0f;

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
            _prerenderProvider.CreatePrerenders(drawArgs, _map.Size);

            // apply view matrix
            drawArgs.PushScale(_zoom);
            drawArgs.PushTranslation(_pan);

            // apply level of detail
            var drawLod = _zoom > 0.5 ? DrawLevelOfDetail.Normal : DrawLevelOfDetail.Low;
            drawArgs.LevelOfDetail = drawLod;

            // calculate viewport transform, used for zoom/pan
            Matrix3x2.Invert(drawArgs.CurrentTransform, out _mapTransform);

            // calculate visible field coords
            var bottomLeftFieldCoords = TranslateToFieldCoords(_bottomLeftViewLimit);
            var blx = bottomLeftFieldCoords.X;
            var bly = bottomLeftFieldCoords.Y;
            var topRightFieldCoords = TranslateToFieldCoords(_topRightViewLimit);
            var urx = topRightFieldCoords.X;
            var ury = topRightFieldCoords.Y;

            // have agents draw themselves layer by layer
            foreach (var drawLayer in DrawLayers)
            {
                drawArgs.Layer = drawLayer;

                var prerender = _prerenderProvider.GetPrerender(drawLod, drawLayer);

                if (prerender != null)
                {
                    drawArgs.Ds.DrawImage(prerender, -_tileRadius, -2.0f * _tileRadiusH);
                }
                else
                {
                    // find indicators which are currently on screen
                    var culledIndicators = _indicatorRegistry
                        .GetIndicators(drawLod, drawLayer)
                        .Where(tm => blx <= tm.Position.X && tm.Position.X <= urx && bly <= tm.Position.Y && tm.Position.Y <= ury);

                    foreach (var indicator in culledIndicators)
                        indicator.Draw(drawArgs);
                }
            }

            // restore view matrix
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
            var newZoom = Math.Max(0.1f, Math.Min(2.0f, _zoom + eventArgs.ZoomDelta));

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
