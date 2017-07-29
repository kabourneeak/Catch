using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerSharedResources : IGraphicsResource
    {
        public IConfig Config { get; }

        public IndicatorCollection Indicators { get; }

        public GunTowerSharedResources(IConfig config)
        {
            Config = config;
            Indicators = new IndicatorCollection();

            Indicators.Add(new TowerTileIndicator(config, Colors.DeepSkyBlue));
            Indicators.Add(new GunTowerBaseIndicator(config));
            Indicators.Add(new GunTowerStrategicIndicator(config));
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            Indicators.CreateResources(args);
        }

        public void DestroyResources()
        {
            Indicators.DestroyResources();
        }
    }
}
