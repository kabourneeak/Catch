using System;
using System.Numerics;
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

        public Vector2 Position { get; set; }

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

        public void Draw(DrawArgs drawArgs)
        {
            if (!_textResource.IsCreated)
                _textResource.CreateResources(drawArgs.ResourceCreator);

            if (!Style.IsCreated)
                Style.CreateResources(drawArgs.ResourceCreator);

            drawArgs.Ds.DrawTextLayout(_textResource.Label, Position + _textResource.Offset, Style.Brush);
        }
    }
}
