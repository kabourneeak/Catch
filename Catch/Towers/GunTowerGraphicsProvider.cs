using System;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerGraphicsProvider : IGraphicsProvider
    {
        public IndicatorCollection Indicators { get; }

        public GunTowerGraphicsProvider(IConfig config, StyleProvider styleProvider)
        {
            Indicators = new IndicatorCollection();

            Indicators.Add(new GunTowerBaseIndicator(config, styleProvider.GetStyle("GunTowerStyle")));
            Indicators.Add(new GunTowerStrategicIndicator(config, styleProvider.GetStyle("GunTowerStyle")));
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
