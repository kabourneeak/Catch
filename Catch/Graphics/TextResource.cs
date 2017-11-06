using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.Graphics
{
    /// <summary>
    /// The resource wrapper by <see cref="TextIndicator"/>, which decouples resource initialization
    /// and management from the use of any particular string by the Label
    /// </summary>
    public class TextResource : IGraphicsResource
    {
        public CanvasTextLayout Label { get; private set; }

        public string Text { get; }

        public Vector2 Offset { get; set; }

        public bool IsCreated => Label != null;

        public TextResource(string text)
        {
            Text = text;
        }

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            DestroyResources();

            using (var format = new CanvasTextFormat())
            {
                // TODO move these options down to the LabelIndicator
                format.VerticalAlignment = CanvasVerticalAlignment.Center;
                format.HorizontalAlignment = CanvasHorizontalAlignment.Center;
                
                Label = new CanvasTextLayout(resourceCreator, Text, format, 100, 100);

                Offset = new Vector2(-50.0f, -50.0f);
            }
        }

        public void DestroyResources()
        {
            if (Label == null)
                return;

            Label.Dispose();
            Label = null;
        }
    }
}
