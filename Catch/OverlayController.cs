using System;
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
    public class OverlayController : IGraphicsResource, IViewportController, IUpdatable
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

        public void CreateResources(CreateResourcesArgs args)
        {
            _statusBar.CreateResources(args);
            _hoverIndicator.CreateResources(args);
        }

        public void DestroyResources()
        {
            _statusBar.DestroyResources();
            _hoverIndicator.DestroyResources();
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
        private ITileAgent _lastHoverTower;
        private readonly StatusBar _statusBar;
        private readonly TowerHoverIndicator _hoverIndicator;

        public void Hover(HoverEventArgs eventArgs)
        {
            if (_lastHover != null && _lastHover.Equals(_level.Ui.HoverHexCoords))
            {
                if (_lastHoverTower == _level.Ui.HoverTower)
                {
                    return;
                }
            }

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
                _lastHoverTower = tower;
            }
            else
            {
                _level.Ui.HoverTower = null;
                _lastHover = null;
                _lastHoverTower = null;
            }
        }

        public void Touch(TouchEventArgs eventArgs)
        {
            // TODO
        }

        public void KeyPress(KeyPressEventArgs eventArgs)
        {
            if (eventArgs.Key >= VirtualKey.Number1 && eventArgs.Key <= VirtualKey.Number9)
            {
                var cmdIndex = eventArgs.Key - VirtualKey.Number1;
                StartAgentCommand(cmdIndex);
            }
        }

        #endregion

        private void StartAgentCommand(int index)
        {
            var tower = _level.Ui.HoverTower;

            if (tower == null)
                return;

            var cmd = tower.Commands.GetCommand(index);

            if (cmd == null)
                return;

            switch (cmd.CommandType)
            {
                case AgentCommandType.Action:
                    cmd.Execute();
                    break;
                case AgentCommandType.Confirm:
                    throw new NotImplementedException();
                case AgentCommandType.List:
                    throw new NotImplementedException();
                case AgentCommandType.SelectTower:
                    throw new NotImplementedException();
                case AgentCommandType.SelectMob:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
