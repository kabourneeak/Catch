using System;
using Catch.Base;
using Catch.Models;
using Catch.Services;

namespace Catch.Win2d
{
    public class Win2DProvider : ITileProvider, IAgentProvider, IMapProvider
    {
        private readonly IConfig _config;
        private readonly float _tileRadius;

        public Win2DProvider(IConfig config)
        {
            _config = config;

            // copy-down config
            _tileRadius = _config.GetFloat("TileRadius");
        }

        #region ITileProvider implementation

        public Tile CreateTile(int row, int col)
        {
            var tile = new Tile(row, col, _config);

            return tile;
        }

        #endregion

        #region IAgentProvider implementation

        public Tower CreateTower(string name, Tile tile)
        {
            if (name == "GunTower")
            {
                return new GunTower(tile, _config);
            }

            throw new ArgumentException("I don't know how to construct that Tower");
        }

        public Mob CreateMob(string name, MapPath mapPath)
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

        public Map CreateMap()
        {
            var map = new Map(this, _config);

            return map;
        }

        #endregion
    }
}
