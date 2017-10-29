using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Map
{
    public class MapGraphicsProvider : IGraphicsProvider
    {
        private readonly TileOutlineIndicator _emptyTileIndicator;

        public IIndicator EmptyTileIndicator => _emptyTileIndicator;

        public MapGraphicsProvider(IConfig config)
        {
            _emptyTileIndicator = new TileOutlineIndicator(config, Colors.DarkRed);           
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            _emptyTileIndicator.CreateResources(args);
        }

        public void DestroyResources()
        {
            _emptyTileIndicator.DestroyResources();
        }
    }
}
