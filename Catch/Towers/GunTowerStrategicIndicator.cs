using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerStrategicIndicator : HexagonIndicator, IIndicator
    {
        public GunTowerStrategicIndicator(IConfig config, IStyle style)
        {
            var radius = config.GetFloat(CoreConfig.TileRadius);
            var inset = config.GetFloat(CoreConfig.TileRadiusInset);

            Radius = radius - inset;
            Style = style;
            Layer = DrawLayer.Tower;
            Filled = true;
            LevelOfDetail = DrawLevelOfDetail.Low;
        }
    }
}
