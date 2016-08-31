using System.Collections.Generic;
using System.Numerics;
using Windows.System;
using Catch.Base;
using Catch.Graphics;
using Catch.LevelUi;
using Catch.Services;
using Catch.Towers;
using CatchLibrary.HexGrid;

namespace Catch
{
    /// <summary>
    /// Handles drawing and event handling for the player control overlay, i.e., the buttons
    /// and things the user will interact with
    /// </summary>
    public class OverlayController : IGraphics, IViewportController, IUpdatable
    {
        private readonly IConfig _config;
        private readonly UiStateModel _ui;
        private readonly List<IAgent> _agents;
        private readonly Map.Map _map;

        public OverlayController(IConfig config, UiStateModel ui, List<IAgent> agents, Map.Map map)
        {
            _config = config;
            _ui = ui;
            _agents = agents;
            _map = map;

            // create UI elements
            _statusBar = new StatusBar(_ui, _config);
            _hoverIndicator = new TowerHoverIndicator(_config);
        }

        public void Initialize()
        {

        }

        #region IUpdatable Implementation

        public void Update(float ticks)
        {
            
        }

        #endregion

        #region IGraphicsComponent Implementation

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            _statusBar.CreateResources(createArgs);
        }

        public void DestroyResources()
        {
            _statusBar.DestroyResources();
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            _statusBar.Draw(drawArgs, rotation);
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

        private HexCoords _lastHover;
        private readonly StatusBar _statusBar;
        private readonly TowerHoverIndicator _hoverIndicator;

        public void Hover(Vector2 viewCoords, VirtualKeyModifiers keyModifiers)
        {
            if (_lastHover != null && _lastHover.Equals(_ui.HoverHexCoords))
                return;

            // remove previous indicator
            _ui.HoverTower?.Indicators.Remove(_hoverIndicator);

            if (_map.HasHex(_ui.HoverHexCoords))
            {
                // add new indicator
                var tile = _map.GetHex(_ui.HoverHexCoords);
                var tower = tile.GetTower();
                tower?.Indicators.Add(_hoverIndicator);

                _ui.HoverTower = tower;
                _lastHover = _ui.HoverHexCoords;
            }
            else
            {
                _ui.HoverTower = null;
                _lastHover = null;
            }
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
