using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Mobs
{
    public class BlockMobSharedResources : IGraphicsResource
    {
        public IndicatorCollection Indicators { get; }

        public BlockMobSharedResources(IConfig config)
        {
            Indicators = new IndicatorCollection
            {
                new BlockMobBaseIndicator(config)
            };
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
