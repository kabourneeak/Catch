using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;

namespace Catch.LevelUi
{
    public class OverlayGraphicsProvider : IGraphicsProvider
    {
        private readonly TileOutlineIndicator _hoverTileIndicator;
        private readonly TileAreaIndicator _selectedTileIndicator;
        private readonly TileAreaIndicator _highlightedTileIndicator;

        public IIndicator HoverTileIndicator => _hoverTileIndicator;

        public IIndicator SelectedTileIndicator => _selectedTileIndicator;

        public IIndicator HighlightedTileIndicator => _highlightedTileIndicator;

        public OverlayGraphicsProvider(IConfig config)
        {
            _hoverTileIndicator = new TileOutlineIndicator(config, Colors.Yellow);
            _selectedTileIndicator = new TileAreaIndicator(config, Colors.LightYellow);
            _highlightedTileIndicator = new TileAreaIndicator(config, Colors.PowderBlue);

        }

        public void CreateResources(CreateResourcesArgs args)
        {
            _hoverTileIndicator.CreateResources(args);
            _selectedTileIndicator.CreateResources(args);
            _highlightedTileIndicator.CreateResources(args);
        }

        public void DestroyResources()
        {
            _hoverTileIndicator.DestroyResources();
            _selectedTileIndicator.DestroyResources();
            _highlightedTileIndicator.DestroyResources();
        }
    }
}
