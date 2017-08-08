using System;
using System.Linq;
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
    public class OverlayController : IGraphicsResource, IViewportController
    {
        private readonly LevelStateModel _level;
        private readonly UiStateModel _uiState;
        private readonly ExecuteEventArgs _executeEventArgs;
        private readonly UpdateReadinessEventArgs _updateReadinessEventArgs;

        public OverlayController(LevelStateModel level, ISimulationManager manager, ISimulationState sim)
        {
            _level = level;
            _uiState = level.Ui;

            _executeEventArgs = new ExecuteEventArgs()
            {
                Manager = manager,
                Sim = sim
            };

            _updateReadinessEventArgs = new UpdateReadinessEventArgs
            {
                Sim = sim
            };

            // create UI elements
            _statusBar = new StatusBar(_uiState);
            _hoverIndicator = new TowerHoverIndicator(_level.Config);
        }

        public void Initialize()
        {

        }

        public void Update(float deviceTicks)
        {
            // copy the focused agent for the duration of this Update/Draw cycle
            _uiState.FocusedAgent = _lastHoverTower;

            if (_uiState.FocusedAgent == null)
                return;

            foreach (var cmd in _uiState.FocusedAgent.Commands)
            {
                cmd.UpdateReadiness(_updateReadinessEventArgs);
            }
        }

        #region IGraphicsResource Implementation

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

        #endregion

        #region IDrawable Implementation

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
            if (_lastHover != null && _lastHover.Equals(_uiState.HoverHexCoords))
            {
                if (_lastHoverTower == _uiState.HoverTower)
                {
                    return;
                }
            }

            // remove previous indicator
            _uiState.HoverTower?.Indicators.Remove(_hoverIndicator);

            if (_level.Map.HasHex(_uiState.HoverHexCoords))
            {
                // add new indicator
                var tile = _level.Map.GetTileModel(_uiState.HoverHexCoords);
                var tower = tile.TileAgent;
                tower?.Indicators.Add(_hoverIndicator);

                _uiState.HoverTower = tower;
                _lastHover = _uiState.HoverHexCoords;
                _lastHoverTower = tower;
            }
            else
            {
                _uiState.HoverTower = null;
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
            if (_uiState.FocusedAgent == null)
                return;

            var cmds = _uiState.FocusedAgent.Commands.Where(c => c.IsVisible).ToArray();
            if (index >= cmds.Length)
                return;

            var cmd = cmds[index];
            if (cmd == null)
                return;

            if (!cmd.IsReady)
                return;

            switch (cmd.CommandType)
            {
                case AgentCommandType.Action:
                    _executeEventArgs.SelectedAgent = null;
                    _executeEventArgs.SelectedTileAgent = null;
                    cmd.Execute(_executeEventArgs);
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
