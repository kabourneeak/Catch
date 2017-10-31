using Catch.Base;
using Catch.Services;

namespace Catch.Graphics
{
    public class ConfiguredHexagonIndicator : HexagonIndicator, IIndicator
    {
        public static readonly string CfgRadius = ConfigUtils.GetConfigPath(nameof(ConfiguredHexagonIndicator), nameof(CfgRadius));
        public static readonly string CfgInset = ConfigUtils.GetConfigPath(nameof(ConfiguredHexagonIndicator), nameof(CfgInset));
        public static readonly string CfgFilled = ConfigUtils.GetConfigPath(nameof(ConfiguredHexagonIndicator), nameof(CfgFilled));
        public static readonly string CfgStyle = ConfigUtils.GetConfigPath(nameof(ConfiguredHexagonIndicator), nameof(CfgStyle));
        public static readonly string CfgLayer = ConfigUtils.GetConfigPath(nameof(ConfiguredHexagonIndicator), nameof(CfgLayer));
        public static readonly string CfgLayerOfDetail = ConfigUtils.GetConfigPath(nameof(ConfiguredHexagonIndicator), nameof(CfgLayerOfDetail));

        public ConfiguredHexagonIndicator(IConfig config, StyleGraphicsProvider styles)
        {
            var radius = config.GetFloat(CoreConfig.TileRadius);
            var inset = config.GetFloat(CoreConfig.TileRadiusInset);

            Radius = radius - inset;
            Style = styles.GetStyle(config.GetString(CfgStyle));
            Layer = DrawLayer.Base;
            LevelOfDetail = DrawLevelOfDetail.All;
        }
    }
}
