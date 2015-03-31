using Catch.Base;
using Catch.Services;

namespace Catch.Win2d
{
    public class Win2DProvider : IHexTileProvider, IAgentProvider, IMapProvider
    {
        private readonly IConfig _config;
        private readonly float _tileRadius;

        public Win2DProvider(IConfig config)
        {
            _config = config;

            // copy-down config
            _tileRadius = _config.GetFloat("TileRadius");
        }

        #region IHexTileProvider implementation

        public IHexTile CreateTile(int row, int col)
        {
            var tile = new BasicHexTile(row, col);
            tile.Graphics = new BasicHexTileGraphics(tile, _tileRadius);

            return tile;
        }

        #endregion

        #region IAgentProvider implementation

        public IAgent CreateAgent(string name)
        {
            throw new System.NotImplementedException();
        }

        public IModifier CreateModifier(string name)
        {
            throw new System.NotImplementedException();
        }

        public IIndicator CreateIndicator(string name)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IMapProvider

        public IMap CreateMap()
        {
            var map = new BasicMap(this);
            map.Graphics = new MapGraphics(map);

            return map;
        }

        #endregion
    }
}
