using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI;
using Catch.Base;
using CatchLibrary.Serialization.Assets;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace Catch.Graphics
{
    public class StyleProvider : IProvider, IGraphicsResourceContainer
    {
        private readonly Dictionary<string, Color> _colors;
        private readonly Dictionary<string, StyleImpl> _styles;

        public StyleProvider(AssetModel assetModel)
        {
            _colors = new Dictionary<string, Color>();
            _styles = new Dictionary<string, StyleImpl>();

            LoadStyles(assetModel);
        }

        public Color GetColor(string colorName)
        {
            if (_colors.TryGetValue(colorName, out var color))
            {
                return color;
            }

            throw new ArgumentException($"The color {colorName} is not defined");
        }

        public IStyle GetStyle(string styleName)
        {
            if (_styles.TryGetValue(styleName, out var style))
            {
                return style;
            }

            throw new ArgumentException($"The style {styleName} is not defined");
        }

        #region IGraphicsResource

        private bool _isCreated;
        private int _createFrameId;

        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || _isCreated == false))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            _createFrameId = args.FrameId;

            foreach (var style in _styles.Values)
            {
                style.Brush = CreateBrush(args, style);
            }

            _isCreated = true;
        }

        public void DestroyResources()
        {
            foreach (var style in _styles.Values)
            {
                if (style.Brush != null)
                {
                    style.Brush.Dispose();
                    style.Brush = null;
                }
            }

            _isCreated = false;
        }

        #endregion

        private void LoadStyles(AssetModel assetModel)
        {
            // load colours
            foreach (var cm in assetModel.Colors)
            {
                var c = ColorFromColorHex(cm.ColorHex);
                _colors.Add(cm.Name, c);
            }

            // load styles
            foreach (var sm in assetModel.Styles)
            {
                // create brush containers that we can return now, and worry about
                // creating the brush itself during the create cycle.

                // use ColorHex if provided, ColorName otherwise
                var color = string.IsNullOrWhiteSpace(sm.ColorHex) 
                    ? GetColor(sm.ColorName) 
                    : ColorFromColorHex(sm.ColorHex);

                _styles.Add(sm.Name, new StyleImpl(sm, color));
            }
        }

        private Color ColorFromColorHex(string colorHex)
        {
            var a = byte.Parse(colorHex.Substring(1, 2), NumberStyles.HexNumber);
            var r = byte.Parse(colorHex.Substring(3, 2), NumberStyles.HexNumber);
            var g = byte.Parse(colorHex.Substring(5, 2), NumberStyles.HexNumber);
            var b = byte.Parse(colorHex.Substring(7, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        private ICanvasBrush CreateBrush(CreateResourcesArgs createArgs, StyleImpl style)
        {
            return CreateBrush(createArgs.ResourceCreator, style);
        }

        private ICanvasBrush CreateBrush(ICanvasResourceCreator resourceCreator, StyleImpl style)
        {
            switch (style.BrushType)
            {
                case BrushType.Solid:
                    return CreateSolidBrush(resourceCreator, style);
                case BrushType.LinearGradient:
                    return CreateLinearGradientBrush(resourceCreator, style);
                case BrushType.RadialGradient:
                    return CreateRadialGradientBrush(resourceCreator, style);
                case BrushType.Image:
                    return CreateImageBrush(resourceCreator, style);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ICanvasBrush CreateSolidBrush(ICanvasResourceCreator resourceCreator, StyleImpl style)
        {
            var brush = new CanvasSolidColorBrush(resourceCreator, style.Color)
            {
                Opacity = style.Opacity
            };

            return brush;
        }

        private ICanvasBrush CreateLinearGradientBrush(ICanvasResourceCreator resourceCreator, StyleImpl style)
        {
            throw new NotImplementedException();
        }

        private ICanvasBrush CreateRadialGradientBrush(ICanvasResourceCreator resourceCreator, StyleImpl style)
        {
            throw new NotImplementedException();
        }

        private ICanvasBrush CreateImageBrush(ICanvasResourceCreator resourceCreator, StyleImpl style)
        {
            throw new NotImplementedException();
        }

        private class StyleImpl : IStyle
        {
            public string Name { get; }

            public Color Color { get; }

            public BrushType BrushType { get; }

            public ICanvasBrush Brush { get; set; }

            public float StrokeWidth { get; }

            public float Opacity { get; }

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
        }

        private enum BrushType
        {
            Solid, LinearGradient, RadialGradient, Image
        }
    }
}
