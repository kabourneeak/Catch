using System;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI.Text;
using Catch.Graphics;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.Level
{
    /// <summary>
    /// A subsidiary of the <see cref="OverlayController"/>
    /// </summary>
    public class StatusBar
    {
        private readonly UiStateModel _uiState;
        private readonly IStyle _fgStyle;
        private readonly IStyle _bgStyle;

        private readonly int _barHeight;
        private CanvasTextFormat _fgTextFormat;

        public StatusBar(UiStateModel uiState, StyleProvider styleProvider)
        {
            _uiState = uiState ?? throw new ArgumentNullException(nameof(uiState));
            if (styleProvider == null) throw new ArgumentNullException(nameof(styleProvider));

            // copy down config
            _barHeight = 26;

            _fgStyle = styleProvider.GetStyle("StatusBarForegroundStyle");
            _bgStyle = styleProvider.GetStyle("StatusBarBackgroundStyle");

            _fgTextFormat = new CanvasTextFormat()
            {
                VerticalAlignment = CanvasVerticalAlignment.Center,
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                FontWeight = FontWeights.Bold,
                FontSize = _barHeight * 0.75f
            };
        }

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.PushTranslation(0, _uiState.WindowSize.Y - _barHeight);

            drawArgs.Ds.FillRectangle(new Rect(0,0, _uiState.WindowSize.X, _barHeight), _bgStyle.Brush);
            drawArgs.Ds.DrawText(GetStatusText(), new Rect(0, 0, _uiState.WindowSize.X, _barHeight), _fgStyle.Brush, _fgTextFormat);

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
