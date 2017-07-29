using System;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Graphics
{
    public enum BrushType
    {
        Solid, LinearGradient, RadialGradient, Image
    }

    public class StyleArgs
    {
        private static readonly CanvasStrokeStyle BaseStrokeStyle = new CanvasStrokeStyle();
        private CanvasStrokeStyle _strokeStyle;

        public Color Color { get; set; }
        public BrushType BrushType { get; set; }
        public float BrushOpacity { get; set; }
        public int StrokeWidth { get; set; }

        public CanvasStrokeStyle StrokeStyle
        {
            get => _strokeStyle ?? BaseStrokeStyle;
            set => _strokeStyle = value;
        }

        /// <summary>
        /// Creates a new StyleArgs object set to White, Solid, 100% opaque, with a strokewidth of 1
        /// </summary>
        public StyleArgs()
        {
            Color = Colors.White;
            BrushType = BrushType.Solid;
            BrushOpacity = 1.0f;
            StrokeWidth = 1;
        }

        public ICanvasBrush CreateBrush(CreateResourcesArgs createArgs)
        {
            return CreateBrush(createArgs.ResourceCreator);
        }

        public ICanvasBrush CreateBrush(ICanvasResourceCreator resourceCreator)
        {
            switch (BrushType)
            {
                case BrushType.Solid:
                    return CreateSolidBrush(resourceCreator);
                case BrushType.LinearGradient:
                    return CreateLinearGradientBrush(resourceCreator);
                case BrushType.RadialGradient:
                    return CreateRadialGradientBrush(resourceCreator);
                case BrushType.Image:
                    return CreateImageBrush(resourceCreator);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ICanvasBrush CreateSolidBrush(ICanvasResourceCreator resourceCreator)
        {
            var brush = new CanvasSolidColorBrush(resourceCreator, Color)
            {
                Opacity = BrushOpacity
            };

            return brush;
        }

        private ICanvasBrush CreateLinearGradientBrush(ICanvasResourceCreator resourceCreator)
        {
            throw new NotImplementedException();
        }

        private ICanvasBrush CreateRadialGradientBrush(ICanvasResourceCreator resourceCreator)
        {
            throw new NotImplementedException();
        }

        private ICanvasBrush CreateImageBrush(ICanvasResourceCreator resourceCreator)
        {
            throw new NotImplementedException();
        }
    }
}
