using Windows.UI;
using Windows.UI.Text;
using Catch.Graphics;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.Level
{
    public class StatusBarGraphicsProvider : IGraphicsProvider
    {
        private readonly StyleArgs _bgStyle;
        private readonly StyleArgs _fgStyle;

        private int _createFrameId = -1;

        public ICanvasBrush BackgroundBrush { get; private set; }

        public ICanvasBrush ForegroundBrush { get; private set; }

        public CanvasTextFormat ForegroundTextFormat { get; }

        public StatusBarGraphicsProvider()
        {
            // copy down config
            var barHeight = 26;

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

            ForegroundTextFormat = new CanvasTextFormat()
            {
                VerticalAlignment = CanvasVerticalAlignment.Center,
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                FontWeight = FontWeights.Bold,
                FontSize = barHeight * 0.75f
            };
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || BackgroundBrush == null))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            BackgroundBrush = _bgStyle.CreateBrush(args);
            ForegroundBrush = _fgStyle.CreateBrush(args);
        }

        public void DestroyResources()
        {
            if (BackgroundBrush == null)
                return;

            BackgroundBrush.Dispose();
            BackgroundBrush = null;

            ForegroundBrush.Dispose();
            ForegroundBrush = null;

            _createFrameId = -1;
        }
    }
}
