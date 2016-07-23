using Windows.UI;
using Catch.Base;
using Catch.Map;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.Graphics
{
    public class LabelIndicator : IIndicator
    {
        private readonly Tile _tile;
        public string Label { get; set; }
        public Color Colour { get; set; }

        public LabelIndicator(Tile tile, string label)
        {
            _tile = tile;
            Label = label;

            Layer = DrawLayer.Ui;
            Colour = Colors.OrangeRed;
        }

        public void Update(float ticks)
        {
            // do nothing
        }

        private int _createFrameId = -1;
        private CanvasTextLayout _label;
        
        public void CreateResources(CreateResourcesArgs createArgs)
        {
            if (!(createArgs.IsMandatory || _label == null))
                return;

            if (_createFrameId == createArgs.FrameId)
                return;

            DestroyResources();

            _createFrameId = createArgs.FrameId;

            var format = new CanvasTextFormat()
            {
                VerticalAlignment = CanvasVerticalAlignment.Center,
                HorizontalAlignment = CanvasHorizontalAlignment.Center
            };

            _label = new CanvasTextLayout(createArgs.ResourceCreator, Label, format, 100, 100);
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

        public DrawLayer Layer { get; set; }
    }
}
