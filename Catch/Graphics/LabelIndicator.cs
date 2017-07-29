using Windows.UI;
using Catch.Base;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.Graphics
{
    public class LabelIndicator : IIndicator, IGraphicsResource
    {
        private string Label { get; }
        private Color Colour { get; }

        public LabelIndicator(string label)
        {
            Label = label;
            Colour = Colors.OrangeRed;
        }

        private int _createFrameId = -1;
        private CanvasTextLayout _label;
        
        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || _label == null))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            _createFrameId = args.FrameId;

            var format = new CanvasTextFormat()
            {
                VerticalAlignment = CanvasVerticalAlignment.Center,
                HorizontalAlignment = CanvasHorizontalAlignment.Center
            };

            _label = new CanvasTextLayout(args.ResourceCreator, Label, format, 100, 100);
        }

        public void DestroyResources()
        {
            if (_label == null)
                return;

            _label.Dispose();
            _label = null;

            _createFrameId = -1;
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            drawArgs.PushScale(1.0f, -1.0f);
            drawArgs.Ds.DrawTextLayout(_label, -50.0f, -50.0f, Colour);
            drawArgs.Pop();
        }

        public DrawLayer Layer => DrawLayer.Ui;

        public DrawLevelOfDetail LevelOfDetail => DrawLevelOfDetail.NormalHigh;
    }
}
