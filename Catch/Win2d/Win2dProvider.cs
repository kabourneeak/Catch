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

        public TowerBase CreateTower(string name, Tile tile, ILevelStateModel level)
        {
            switch (name)
            {
                case nameof(GunTower):
                    return new GunTower(tile, level);
                case nameof(EmptyTower):
                    return new EmptyTower(tile, level);
                default:
                    throw new ArgumentException("I don't know how to construct that Tower");
            }
        }

        public MobBase CreateMob(string name, MapPath mapPath, ILevelStateModel level)
        {
            switch (name)
            {
                case nameof(BlockMob):
                    return new BlockMob(mapPath, level);
                default:
                    throw new ArgumentException("I don't know how to construct that Mob");
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

        public Map.Map CreateMap(int rows, int columns)
        {
            var map = new Map.Map(_config, rows, columns);

            return map;
        }

        #endregion
    }
}
