using System;
using Catch.Base;
using Catch.Map;
using Catch.Mobs;
using Catch.Services;
using Catch.Towers;

namespace Catch.Win2d
{
    public class Win2DProvider : IAgentProvider, IMapProvider
    {
        private readonly IConfig _config;

        public Win2DProvider(IConfig config)
        {
            _config = config;
        }

        #region IAgentProvider implementation

        public TowerBase CreateTower(string name, Tile tile)
        {
            switch (name)
            {
                case nameof(GunTower):
                    return new GunTower(tile, _config);
                case nameof(VoidTower):
                    return new VoidTower(tile, _config);
                case nameof(NilTower):
                    return new NilTower(tile);
                default:
                    throw new ArgumentException("I don't know how to construct that Tower");
            }
        }

        public MobBase CreateMob(string name, MapPath mapPath)
        {
            switch (name)
            {
                case nameof(BlockMob):
                    return new BlockMob(mapPath, _config);
                default:
                    throw new ArgumentException("I don't know how to construct that PathAgent");
            }
        }

        public Modifier CreateModifier(string name)
        {
            throw new NotImplementedException();
        }

        public IIndicator CreateIndicator(string name)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMapProvider

        public Map.Map CreateMap()
        {
            var map = new Map.Map(_config);

            return map;
        }

        #endregion
    }
}
