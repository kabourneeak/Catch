using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerSharedResources : IGraphicsResource
    {
        public IndicatorCollection Indicators { get; }

        public GunTowerSharedResources(IConfig config)
        {
            Indicators = new IndicatorCollection();

            Indicators.Add(new TowerTileIndicator(config, Colors.DeepSkyBlue));
            Indicators.Add(new GunTowerBaseIndicator(config));
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
