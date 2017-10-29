using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Mobs
{
    public class BlockMobGraphicsProvider : IGraphicsProvider
    {
        public IndicatorCollection Indicators { get; }

        public BlockMobGraphicsProvider(IConfig config)
        {
            Indicators = new IndicatorCollection
            {
                new BlockMobBaseIndicator(config)
            };
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
