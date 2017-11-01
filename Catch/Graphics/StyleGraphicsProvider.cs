using System;
using System.Collections.Generic;
using Windows.UI;
using CatchLibrary.Serialization.Assets;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace Catch.Graphics
{
    public class StyleGraphicsProvider : IGraphicsProvider
    {
        private readonly Dictionary<string, Color> _colors;
        private readonly Dictionary<string, BrushImpl> _brushes;

        public StyleGraphicsProvider(AssetModel assetModel)
        {
            _colors = new Dictionary<string, Color>();
            _brushes = new Dictionary<string, BrushImpl>();

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

        public IBrush GetBrush(string styleName)
        {
            if (_brushes.TryGetValue(styleName, out var brush))
            {
                return brush;
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

            foreach (var brush in _brushes.Values)
            {
                brush.Brush = CreateBrush(args, brush.Style);
            }

            _isCreated = true;
        }

        public void DestroyResources()
        {
            foreach (var brush in _brushes.Values)
                brush.Brush.Dispose();
                
            _brushes.Clear();
        }

        #endregion

        private void LoadStyles(AssetModel assetModel)
        {
            // load colours
            foreach (var cm in assetModel.Colors)
            {
                var c = Color.FromArgb(cm.A, cm.R, cm.G, cm.B);
                _colors.Add(cm.Name, c);
            }

            // load styles
            foreach (var sm in assetModel.Styles)
            {
                var style = new StyleArgs
                {                   
                    BrushType = BrushType.Solid,  // TODO support other brush types
                    BrushOpacity = sm.BrushOpacity,
                    StrokeWidth = sm.StrokeWidth,
                    Color = GetColor(sm.ColorName)
                    // TODO support brush style
                };

                // create brush containers that we can return now, and worry about
                // creating the brush itself during the create cycle.
                _brushes.Add(sm.Name, new BrushImpl(sm.Name, style));
            }
        }

        private ICanvasBrush CreateBrush(CreateResourcesArgs createArgs, StyleArgs style)
        {
            return CreateBrush(createArgs.ResourceCreator, style);
        }

        private ICanvasBrush CreateBrush(ICanvasResourceCreator resourceCreator, StyleArgs style)
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

        private ICanvasBrush CreateSolidBrush(ICanvasResourceCreator resourceCreator, StyleArgs style)
        {
            var brush = new CanvasSolidColorBrush(resourceCreator, style.Color)
            {
                Opacity = style.BrushOpacity
            };

            return brush;
        }

        private ICanvasBrush CreateLinearGradientBrush(ICanvasResourceCreator resourceCreator, StyleArgs style)
        {
            throw new NotImplementedException();
        }

        private ICanvasBrush CreateRadialGradientBrush(ICanvasResourceCreator resourceCreator, StyleArgs style)
        {
            throw new NotImplementedException();
        }

        private ICanvasBrush CreateImageBrush(ICanvasResourceCreator resourceCreator, StyleArgs style)
        {
            throw new NotImplementedException();
        }

        private class BrushImpl : IBrush
        {
            public string Name { get; }

            public StyleArgs Style { get; }

            public ICanvasBrush Brush { get; set; }

            public BrushImpl(string name, StyleArgs style)
            {
                Name = name;
                Style = style;
            }
        }

    }
}
