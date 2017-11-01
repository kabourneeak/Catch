using System;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;

namespace Catch.Level
{
    public class OverlayGraphicsProvider : IProvider, IGraphicsResourceContainer
    {
        private readonly TileOutlineIndicator _hoverTileIndicator;
        private readonly TileAreaIndicator _selectedTileIndicator;
        private readonly TileAreaIndicator _highlightedTileIndicator;

        public IIndicator HoverTileIndicator => _hoverTileIndicator;

        public IIndicator SelectedTileIndicator => _selectedTileIndicator;

        public IIndicator HighlightedTileIndicator => _highlightedTileIndicator;

        public OverlayGraphicsProvider(IConfig config, StyleProvider styleProvider)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _hoverTileIndicator = new TileOutlineIndicator(config, styleProvider.GetStyle("HoverTileStyle"));
            _selectedTileIndicator = new TileAreaIndicator(config, styleProvider.GetStyle("SelectedTileStyle"));
            _highlightedTileIndicator = new TileAreaIndicator(config, styleProvider.GetStyle("SelectedTileStyle"));
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
