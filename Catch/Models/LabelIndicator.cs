using Windows.UI;
using Windows.UI.Text;
using Catch.Base;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
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

            var format = new CanvasTextFormat()
            {
                VerticalAlignment = CanvasVerticalAlignment.Center,
                ParagraphAlignment = ParagraphAlignment.Center
            };

            _label = new CanvasTextLayout(createArgs.ResourceCreator, Label, format, 100, 100);
        }

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.PushScale(1.0f, -1.0f);
            drawArgs.Ds.DrawTextLayout(_label, -50.0f, -50.0f, Colour);
            drawArgs.Pop();
        }

        public DrawLayer Layer { get; set; }
    }
}
