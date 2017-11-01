using Windows.UI.Text;
using Catch.Graphics;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.Level
{
    public class StatusBarGraphicsProvider : IGraphicsProvider
    {
        public IStyle BackgroundStyle { get; private set; }

        public IStyle ForegroundStyle { get; private set; }

        public CanvasTextFormat ForegroundTextFormat { get; }

        public StatusBarGraphicsProvider(StyleProvider styleProvider)
        {
            // copy down config
            var barHeight = 26;

            BackgroundStyle = styleProvider.GetStyle("StatusBarBackgroundStyle");
            ForegroundStyle = styleProvider.GetStyle("StatusBarForegroundStyle");

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
            // do nothing
        }

        public void DestroyResources()
        {
            // do nothing
        }
    }
}
