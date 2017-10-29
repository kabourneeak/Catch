using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTower : AgentBase
    {
        public GunTower(IConfig config, GunTowerGraphicsProvider resources, IMapTile tile) : base(nameof(GunTower))
        {
        }
    }
}
