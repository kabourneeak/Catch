﻿using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Catch.Graphics;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.LevelUi
{
    public class StatusBar : IGraphicsResource
    {
        private readonly ILevelStateModel _level;

        private readonly int _barHeight;
        private readonly StyleArgs _bgStyle;
        private readonly StyleArgs _fgStyle;
        private readonly CanvasTextFormat _fgTextFormat;

        public StatusBar(ILevelStateModel level)
        {
            _level = level;

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

        #region IGraphics implementation

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

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            drawArgs.PushTranslation(0, _level.Ui.WindowSize.Y - _barHeight);

            drawArgs.Ds.FillRectangle(new Rect(0,0, _level.Ui.WindowSize.X, _barHeight), _bgBrush);
            drawArgs.Ds.DrawText(GetStatusText(), new Rect(0, 0, _level.Ui.WindowSize.X, _barHeight), _fgBrush, _fgTextFormat);

            drawArgs.Pop();
        }

        #endregion

        private string GetStatusText()
        {
            if (_level.Ui.HoverTower != null)
            {
                var sb = new StringBuilder();
                var tower = _level.Ui.HoverTower;
                sb.Append(tower.DisplayName);
                sb.Append(": ");
                sb.Append(tower.DisplayStatus);

                var cmdIndex = 0;

                foreach (var cmd in tower.Commands)
                {
                    sb.Append("(");
                    sb.Append(cmdIndex + 1);
                    sb.Append(")");
                    sb.Append(cmd.DisplayName);
                }

                return sb.ToString();
            }

            // if nothing hovered, show level info, player status?

            return string.Empty;
        }
    }
}
