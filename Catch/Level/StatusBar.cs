using System;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Catch.Graphics;

namespace Catch.Level
{
    /// <summary>
    /// A subsidiary of the <see cref="OverlayController"/>
    /// </summary>
    public class StatusBar
    {
        private readonly UiStateModel _uiState;
        private readonly StatusBarGraphicsProvider _graphicsProvider;

        private readonly int _barHeight;

        public StatusBar(UiStateModel uiState, StatusBarGraphicsProvider graphicsProvider)
        {
            _uiState = uiState ?? throw new ArgumentNullException(nameof(uiState));
            _graphicsProvider = graphicsProvider ?? throw new ArgumentNullException(nameof(graphicsProvider));

            // copy down config (fix in StatusBarGraphicsProvider, too)
            _barHeight = 26;
        }

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.PushTranslation(0, _uiState.WindowSize.Y - _barHeight);

            drawArgs.Ds.FillRectangle(new Rect(0,0, _uiState.WindowSize.X, _barHeight), _graphicsProvider.BackgroundBrush);
            drawArgs.Ds.DrawText(GetStatusText(), new Rect(0, 0, _uiState.WindowSize.X, _barHeight), _graphicsProvider.ForegroundBrush, _graphicsProvider.ForegroundTextFormat);

            drawArgs.Pop();
        }

        private string GetStatusText()
        {
            var tileAgent = _uiState.FocusedAgent;

            if (tileAgent == null)
                return string.Empty;

            var sb = new StringBuilder();
            sb.Append(tileAgent.Stats.DisplayName);
            sb.Append(": ");
            sb.Append(tileAgent.Stats.DisplayStatus);

            var cmdIndex = 0;

            foreach (var cmd in tileAgent.Commands.Where(c => c.IsVisible))
            {
                sb.Append("(");
                sb.Append(++cmdIndex);
                sb.Append(")");
                sb.Append(cmd.DisplayName);
            }

            return sb.ToString();
        }
    }
}
