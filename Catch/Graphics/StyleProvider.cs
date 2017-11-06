using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI;
using Catch.Base;
using CatchLibrary.Serialization.Assets;
using Microsoft.Graphics.Canvas;

namespace Catch.Graphics
{
    public class StyleProvider : IProvider, IGraphicsResourceContainer
    {
        private readonly Dictionary<string, Color> _colors;
        private readonly Dictionary<string, StyleImpl> _styles;

        private ICanvasResourceCreator _resourceCreator;

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

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            DestroyResources();

            _resourceCreator = resourceCreator;

            foreach (var style in _styles.Values)
                style.CreateResources(resourceCreator);
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
    }
}
