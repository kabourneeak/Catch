﻿using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI;
using Catch.Services;
using CatchLibrary.Serialization;
using Newtonsoft.Json;

namespace Catch.Graphics
{
    public class StyleGraphicsProvider : IGraphicsProvider
    {
        private static readonly string CfgStylesJson = ConfigUtils.GetConfigPath(nameof(StyleGraphicsProvider), nameof(CfgStylesJson));

        private readonly Dictionary<string, Color> _colors;
        private readonly Dictionary<string, StyleArgs> _styles;

        public StyleGraphicsProvider(IConfig config)
        {
            _colors = new Dictionary<string, Color>();
            _styles = new Dictionary<string, StyleArgs>();

            var fileName = config.GetString(CfgStylesJson);
            LoadStyles(fileName);
        }

        public Color GetColor(string colorName)
        {
            if (_colors.TryGetValue(colorName, out var color))
            {
                return color;
            }

            throw new ArgumentException($"The color {colorName} is not defined");
        }

        public StyleArgs GetStyle(string styleName)
        {
            if (_styles.TryGetValue(styleName, out var style))
            {
                return style;
            }

            throw new ArgumentException($"The style {styleName} is not defined");
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            // do nothing
        }

        public void DestroyResources()
        {
            // no nothing
        }

        private void LoadStyles(string fileName)
        {
            var stylePack = JsonConvert.DeserializeObject<StylePackModel>(File.ReadAllText(fileName));

            // load colours
            foreach (var cm in stylePack.Colors)
            {
                var c = Color.FromArgb(cm.A, cm.R, cm.G, cm.B);
                _colors.Add(cm.Name, c);
            }

            // load styles
            foreach (var sm in stylePack.Styles)
            {
                var style = new StyleArgs
                {                   
                    BrushType = BrushType.Solid,  // TODO support other brush types
                    BrushOpacity = sm.BrushOpacity,
                    StrokeWidth = sm.StrokeWidth,
                    Color = GetColor(sm.ColorName)
                    // TODO support brush style
                };

                _styles.Add(sm.Name, style);
            }
        }
    }
}