using System;
using Catch.Base;
using Catch.Models;
using Catch.Services;

namespace Catch.Win2d
{
    public class Win2DProvider : IAgentProvider, IMapProvider
    {
        private readonly IConfig _config;
        private readonly float _tileRadius;

        public Win2DProvider(IConfig config)
        {
            _config = config;

            // copy-down config
            _tileRadius = _config.GetFloat("TileRadius");
        }

        #region IAgentProvider implementation

        public Tower CreateTower(string name, Tile tile)
        {
            if (name == typeof(GunTower).Name)
            {
                return new GunTower(tile, _config);
            }
            else if (name == typeof(VoidTower).Name)
            {
                return new VoidTower(tile, _config);
            }
            else if (name == typeof (NilTower).Name)
            {
                return new NilTower(tile);
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

        public Modifier CreateModifier(string name)
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
            var map = new Map(_config);

            return map;
        }

        #endregion
    }
}
