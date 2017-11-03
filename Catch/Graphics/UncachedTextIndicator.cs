using System;
using System.Numerics;
using Catch.Base;
using Catch.Services;
using Microsoft.Graphics.Canvas.Text;

namespace Catch.Graphics
{
    [Obsolete]
    public class UncachedTextIndicator : IIndicator
    {
        private static readonly string CfgStyleName = ConfigUtils.GetConfigPath(nameof(UncachedTextIndicator), nameof(CfgStyleName));
        private static readonly string CfgLayer = ConfigUtils.GetConfigPath(nameof(UncachedTextIndicator), nameof(CfgLayer));
        private static readonly string CfgLevelOfDetail = ConfigUtils.GetConfigPath(nameof(UncachedTextIndicator), nameof(CfgLevelOfDetail));

        private string _labelText;

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public string Label => _labelText;

        public IStyle Style { get; }

        public DrawLayer Layer { get; }

        public DrawLevelOfDetail LevelOfDetail { get; }

        public UncachedTextIndicator(IConfig config, StyleProvider styleProvider)
        {
            // TODO add more configuration for text size, etc

            _labelText = string.Empty;
            Style = styleProvider.GetStyle(config.GetString(CfgStyleName));

            var strCfgLayer = config.GetString(CfgLayer);
            if (Enum.TryParse(strCfgLayer, out DrawLayer layer))
            {
                Layer = layer;
            }
            else
            {
                throw new ArgumentException($"Could not parse {strCfgLayer} as DrawLayer");
            }

            var strCfgLod = config.GetString(CfgLevelOfDetail);
            if (Enum.TryParse(strCfgLod, out DrawLevelOfDetail lod))
            {
                LevelOfDetail = lod;
            }
            else
            {
                throw new ArgumentException($"Could not parse {strCfgLod} as DrawLevelOfDetail");
            }

        }
        public void SetLabelText(string text)
        {
            _labelText = text.Trim();
        }

        public void Draw(DrawArgs drawArgs)
        {
            if (Label.Length > 0)
            {
                using (var format = new CanvasTextFormat())
                {
                    // TODO set formatting options
                    format.VerticalAlignment = CanvasVerticalAlignment.Center;
                    format.HorizontalAlignment = CanvasHorizontalAlignment.Center;

                    drawArgs.PushScale(1.0f, -1.0f);
                    drawArgs.Ds.DrawText(_labelText, -50.0f, -50.0f, Style.Brush, format);
                    drawArgs.Pop();
                }
            }
        }
    }
}
