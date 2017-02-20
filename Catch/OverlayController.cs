using System.Collections.Generic;
using System.Numerics;
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
        private readonly ILevelStateModel _level;
        private readonly List<IAgent> _agents;

        public OverlayController(ILevelStateModel level, List<IAgent> agents)
        {
            _level = level;
            _agents = agents;

            // create UI elements
            _statusBar = new StatusBar(_level);
            _hoverIndicator = new TowerHoverIndicator(_level.Config);
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

        public void PanBy(PanByEventArgs eventArgs)
        {
            // do nothing
        }

        public void ZoomToPoint(ZoomToPointEventArgs eventArgs)
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

        public void Hover(HoverEventArgs eventArgs)
        {
            if (_lastHover != null && _lastHover.Equals(_level.Ui.HoverHexCoords))
                return;

            // remove previous indicator
            _level.Ui.HoverTower?.Indicators.Remove(_hoverIndicator);

            if (_level.Map.HasHex(_level.Ui.HoverHexCoords))
            {
                // add new indicator
                var tile = _level.Map.GetHex(_level.Ui.HoverHexCoords);
                var tower = tile.GetTower();
                tower?.Indicators.Add(_hoverIndicator);

                _level.Ui.HoverTower = tower;
                _lastHover = _level.Ui.HoverHexCoords;
            }
            else
            {
                _level.Ui.HoverTower = null;
                _lastHover = null;
            }
        }

        public void Touch(TouchEventArgs eventArgs)
        {
            // TODO
        }

        public void KeyPress(KeyPressEventArgs eventArgs)
        {
            // TODO
        }

        #endregion
    }
}
