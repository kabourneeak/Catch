using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Graphics
{
    public class TextIndicator : IIndicator
    {
        private static readonly string CfgStyleName = ConfigUtils.GetConfigPath(nameof(TextIndicator), nameof(CfgStyleName));
        private static readonly string CfgLayer = ConfigUtils.GetConfigPath(nameof(TextIndicator), nameof(CfgLayer));
        private static readonly string CfgLevelOfDetail = ConfigUtils.GetConfigPath(nameof(TextIndicator), nameof(CfgLevelOfDetail));

        private readonly TextResourceProvider _labelProvider;
        private string _labelText;
        private TextResource _textResource;

        public string Label => _labelText;

        public IStyle Style { get; }

        public DrawLayer Layer { get; }

        public DrawLevelOfDetail LevelOfDetail { get; }

        public TextIndicator(IConfig config, TextResourceProvider labelProvider, StyleProvider styleProvider)
        {
            // TODO add more configuration for text size, etc

            _labelText = string.Empty;
            _labelProvider = labelProvider;
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
            _textResource = _labelProvider.GetLabel(_labelText);
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            if (Label.Length > 0)
            {
                drawArgs.PushScale(1.0f, -1.0f);
                drawArgs.Ds.DrawTextLayout(_textResource.Label, -50.0f, -50.0f, Style.Brush);
                drawArgs.Pop();
            }
        }
    }
}
