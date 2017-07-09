using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Mobs;
using Catch.Services;
using Catch.Towers;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    public class Tile
    {
        private ITileAgent _tileAgent;
        private readonly ISet<MobBase> _mobs;

        public Tile(HexCoords coords, Map map, IConfig config)
        {
            Map = map;
            Coords = coords;

            _mobs = new HashSet<MobBase>();

            // copy down config
            var radius = config.GetFloat("TileRadius");

            // calculate position
            var radiusH = HexUtils.GetRadiusHeight(radius);

            var x = Coords.Column * (radius + radius * HexUtils.COS60);
            var y = ((Coords.Column & 1) * radiusH) + ((Coords.Row - (Coords.Column & 1)) * 2 * radiusH);

            Position = new Vector2(x, y);
        }

        public Map Map { get; }

        public HexCoords Coords { get; }

        public Vector2 Position { get; }

        #region Tower Management

        public ITileAgent GetTower()
        {
            return _tileAgent;
        }

        public bool HasTower()
        {
            return _tileAgent != null;
        }

        public bool RemoveTower(TowerBase tower)
        {
            if (_tileAgent == tower)
            {
                _tileAgent = null;
                return true;
            }

            return false;
        }

        public bool AddTower(ITileAgent tileAgent)
        {
            if (_tileAgent == tileAgent)
                return false;

            if (_tileAgent != null)
            {
                // TODO send OnRemove event
            }

            _tileAgent = tileAgent;

            return true;
        }

        #endregion

        #region Mob Management 

        public bool AddMob(MobBase mob)
        {
            return _mobs.Add(mob);
        }

        public bool ContainsMob(MobBase mob)
        {
            return _mobs.Contains(mob);
        }

        public bool RemoveMob(MobBase mob)
        {
            return _mobs.Remove(mob);
        }

        public int MobCount => _mobs.Count;

        public IEnumerable<MobBase> Mobs => _mobs;

        #endregion

        public override string ToString()
        {
            return $"Tile {Coords}";
        }
    }
}
