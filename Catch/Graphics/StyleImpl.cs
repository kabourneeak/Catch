using System;
using Windows.UI;
using CatchLibrary.Serialization.Assets;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace Catch.Graphics
{
    public class StyleImpl : IStyle, IGraphicsResource
    {
        private ICanvasBrush _brush;

        public string Name { get; }

        public Color Color { get; }

        public ICanvasBrush Brush => _brush;

        public float StrokeWidth { get; }

        private BrushType BrushType { get; }

        private float Opacity { get; }

        public StyleImpl(StyleModel style, Color color)
        {
            Name = style.Name;
            StrokeWidth = style.StrokeWidth;
            Opacity = style.BrushOpacity;
            Color = color;

            if (Enum.TryParse(style.BrushType, out BrushType bt))
            {
                BrushType = bt;
            }
            else
            {
                throw new ArgumentException($"Could not parse BrushType {style.BrushType}");
            }
        }

        public bool IsCreated => _brush != null;

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            DestroyResources();

            _brush = CreateBrush(resourceCreator);
        }

        public void DestroyResources()
        {
            if (_brush != null)
            {
                _brush.Dispose();
                _brush = null;
            }
        }

        private ICanvasBrush CreateBrush(ICanvasResourceCreator resourceCreator)
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
                Opacity = Opacity
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
