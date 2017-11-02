using Microsoft.Graphics.Canvas.Text;

namespace Catch.Graphics
{
    /// <summary>
    /// The resource wrapper by <see cref="TextIndicator"/>, which decouples resource initialization
    /// and management from the use of any particular string by the Label
    /// </summary>
    public class TextResource : IGraphicsResource
    {
        private int _createFrameId = -1;

        public CanvasTextLayout Label { get; private set; }

        public string Text { get; }

        public TextResource(string text)
        {
            Text = text;
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || Label == null))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            _createFrameId = args.FrameId;

            using (var format = new CanvasTextFormat())
            {
                // TODO move these options down to the LabelIndicator
                format.VerticalAlignment = CanvasVerticalAlignment.Center;
                format.HorizontalAlignment = CanvasHorizontalAlignment.Center;
                
                Label = new CanvasTextLayout(args.ResourceCreator, Text, format, 100, 100);
            }
        }

        public void DestroyResources()
        {
            if (Label == null)
                return;

            Label.Dispose();
            Label = null;

            _createFrameId = -1;
        }
    }
}
