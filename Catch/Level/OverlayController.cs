using System;
using System.Linq;
using System.Numerics;
using Windows.System;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using CatchLibrary.HexGrid;

namespace Catch.Level
{
    /// <summary>
    /// Handles drawing and event handling for the player control overlay, i.e., the buttons
    /// and things the user will interact with
    /// </summary>
    public class OverlayController : IViewportController
    {
        private readonly UiStateModel _uiState;
        private readonly ExecuteEventArgs _executeEventArgs;
        private readonly UpdateReadinessEventArgs _updateReadinessEventArgs;
        private readonly StatusBar _statusBar;

        private HexCoords _lastHover;
        private MapTileModel _lastHoverTile;
        private IIndicator _hoverTileIndicator;

        public OverlayController(UiStateModel uiState,
            ISimulationManager simulationManager,
            ISimulationState simulationState,
            IndicatorProvider indicatorProvider,
            StatusBar statusBar)
        {
            _uiState = uiState ?? throw new ArgumentNullException(nameof(uiState));
            if (simulationManager == null) throw new ArgumentNullException(nameof(simulationManager));
            if (simulationState == null) throw new ArgumentNullException(nameof(simulationState));
            if (indicatorProvider == null) throw new ArgumentNullException(nameof(indicatorProvider));
            _statusBar = statusBar ?? throw new ArgumentNullException(nameof(statusBar));

            _executeEventArgs = new ExecuteEventArgs()
            {
                Manager = simulationManager,
                Sim = simulationState,
            };

            _updateReadinessEventArgs = new UpdateReadinessEventArgs
            {
                Sim = simulationState,
            };

            _lastHover = HexCoords.CreateFromOffset(-1, -1);
            _hoverTileIndicator = indicatorProvider.GetIndicator("HoverTileIndicator");
        }

        public void Initialize()
        {

        }

        public void Update(float deviceTicks)
        {
            // copy the focused agent for the duration of this Update/Draw cycle
            _uiState.FocusedAgent = _lastHoverTile?.ExtendedTileAgent;

            if (_uiState.FocusedAgent == null)
                return;

            foreach (var cmd in _uiState.FocusedAgent.Commands)
            {
                cmd.UpdateReadiness(_updateReadinessEventArgs);
            }
        }

        #region IDrawable Implementation

        public void Draw(DrawArgs drawArgs)
        {
            _statusBar.Draw(drawArgs);
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

        public void Hover(HoverEventArgs eventArgs)
        {
            // we are still hovering over the same coordinates as last time, 
            // nothing to do
            if (_lastHover.Equals(_uiState.HoverHexCoords))
                return;

            // remove previous indicator
            _lastHoverTile?.Indicators.Remove(_hoverTileIndicator);

            // add new indicator
            _lastHover = _uiState.HoverHexCoords;
            _lastHoverTile = _uiState.HoverTile;
            _lastHoverTile?.Indicators.Add(_hoverTileIndicator);
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
