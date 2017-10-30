using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Map
{
    public class MapGraphicsProvider : IGraphicsProvider
    {
        private readonly TileOutlineIndicator _emptyTileIndicator;
        private readonly TileAreaIndicator _pathTileIndicator;

        public IIndicator EmptyTileIndicator => _emptyTileIndicator;

        public IIndicator PathTileIndicator => _pathTileIndicator;

        public MapGraphicsProvider(IConfig config)
        {
            _emptyTileIndicator = new TileOutlineIndicator(config, Colors.DarkRed);
            _pathTileIndicator = new TileAreaIndicator(config, Colors.CornflowerBlue);
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            _emptyTileIndicator.CreateResources(args);
            _pathTileIndicator.CreateResources(args);
        }

        public void DestroyResources()
        {
            _emptyTileIndicator.DestroyResources();
            _pathTileIndicator.DestroyResources();
        }
    }
}
