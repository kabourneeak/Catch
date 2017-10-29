using System;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Catch.Graphics;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.Level
{
    public class StatusBar : IGraphicsResource
    {
        private readonly UiStateModel _uiState;

        private readonly int _barHeight;
        private readonly StyleArgs _bgStyle;
        private readonly StyleArgs _fgStyle;
        private readonly CanvasTextFormat _fgTextFormat;

        public StatusBar(UiStateModel uiState)
        {
            _uiState = uiState ?? throw new ArgumentNullException(nameof(uiState));

            // copy down config
            _barHeight = 26;

            _bgStyle = new StyleArgs()
            {
                Color = Color.FromArgb(0xFF, 0xAA, 0x84, 0x39),
                BrushOpacity = 0.75f
            };

            _fgStyle = new StyleArgs()
            {
                Color = Color.FromArgb(0xFF, 0x37, 0x21, 0x44),
                BrushOpacity = 1.0f
            };

            _fgTextFormat = new CanvasTextFormat()
            {
                VerticalAlignment = CanvasVerticalAlignment.Center,
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                FontWeight = FontWeights.Bold,
                FontSize = _barHeight * 0.75f
            };
        }

        #region IGraphicsResource implementation

        private int _createFrameId = -1;
        private ICanvasBrush _bgBrush;
        private ICanvasBrush _fgBrush;

        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || _bgBrush == null))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            _bgBrush = _bgStyle.CreateBrush(args);
            _fgBrush = _fgStyle.CreateBrush(args);
        }

        public void DestroyResources()
        {
            if (_bgBrush == null)
                return;

            _bgBrush.Dispose();
            _bgBrush = null;

            _fgBrush.Dispose();
            _fgBrush = null;

            _createFrameId = -1;
        }

        #endregion

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.PushTranslation(0, _uiState.WindowSize.Y - _barHeight);

            drawArgs.Ds.FillRectangle(new Rect(0,0, _uiState.WindowSize.X, _barHeight), _bgBrush);
            drawArgs.Ds.DrawText(GetStatusText(), new Rect(0, 0, _uiState.WindowSize.X, _barHeight), _fgBrush, _fgTextFormat);

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
