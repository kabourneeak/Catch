using System;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerGraphicsProvider : IGraphicsProvider
    {
        [Obsolete]
        public IConfig Config { get; }

        public IndicatorCollection Indicators { get; }

        public GunTowerGraphicsProvider(IConfig config)
        {
            Config = config;
            Indicators = new IndicatorCollection();

            Indicators.Add(new GunTowerBaseIndicator(config));
            Indicators.Add(new GunTowerStrategicIndicator(config));
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            foreach (var indicator in Indicators)
                indicator.CreateResources(args);
        }

        public void DestroyResources()
        {
            foreach (var indicator in Indicators)
                indicator.DestroyResources();
        }
    }
}
