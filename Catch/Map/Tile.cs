using System.Collections.Generic;
using System.Numerics;
using Catch.Mobs;
using Catch.Services;
using Catch.Towers;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    public class Tile
    {
        private TowerBase _tower;
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

        public Map Map { get; protected set; }

        public HexCoords Coords { get; protected set; }

        public Vector2 Position { get; protected set; }

        #region Tower Management

        public TowerBase GetTower()
        {
            return _tower;
        }

        public bool HasTower()
        {
            return _tower != null;
        }

        public bool RemoveTower(TowerBase tower)
        {
            if (_tower == tower)
            {
                _tower = null;
                return true;
            }

            return false;
        }

        public bool AddTower(TowerBase tower)
        {
            if (_tower == tower)
                return false;

            if (_tower != null)
            {
                // TODO send OnRemove event
            }

            _tower = tower;

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

        public int MobCount {get { return _mobs.Count; }}

        public IEnumerable<MobBase> Mobs { get { return _mobs; } }

        #endregion

        public override string ToString()
        {
            return $"Tile {Coords}";
        }
    }
}
