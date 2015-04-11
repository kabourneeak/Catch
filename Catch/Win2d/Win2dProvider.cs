using System;
using Catch.Base;
using Catch.Models;
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
            var tile = new BasicHexTile(row, col, _config);

            return tile;
        }

        #endregion

        #region IAgentProvider implementation

        public ITower CreateTower(string name, IHexTile tile)
        {
            if (name == "GunTower")
            {
                return new GunTower(tile, _config);
            }

            throw new ArgumentException("I don't know how to construct that Tower");
        }

        public IMob CreateMob(string name, MapPath mapPath)
        {
            if (name == "BlockMob")
            {
                return new BlockMob(mapPath, _config);
            }
            
            throw new ArgumentException("I don't know how to construct that PathAgent");
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
            var map = new BasicMap(this, _config);

            return map;
        }

        #endregion
    }
}
