using System.Collections.Generic;
using System.Numerics;
using Catch.Services;

namespace Catch.Base
{
    public class Tile
    {
        private Tower _tower;
        private readonly ISet<Mob> _mobs;

        public Tile(int row, int col, Map map, IConfig config)
        {
            Map = map;
            Row = row;
            Column = col;

            _mobs = new HashSet<Mob>();

            // copy down config
            var radius = config.GetFloat("TileRadius");

            // calculate position
            var radiusH = HexUtils.GetRadiusHeight(radius);

            var x = radius + (col * (radius + radius * HexUtils.COS60));
            var y = (col % 2 * radiusH) + (row * 2 * radiusH) + radiusH;

            Position = new Vector2(x, y);
        }

        #region Tile Implementation

        public Map Map { get; protected set; }

        public int Row { get; protected set; }

        public int Column { get; protected set; }

        public Vector2 Position { get; protected set; }

        public Tower GetTower()
        {
            return _tower;
        }

        public bool HasTower()
        {
            return _tower != null;
        }

        public void SetTower(Tower tower)
        {
            _tower = tower;
        }

        #endregion

        #region Mob Management 

        public bool AddMob(Mob mob)
        {
            return _mobs.Add(mob);
        }

        public bool ContainsMob(Mob mob)
        {
            return _mobs.Contains(mob);
        }

        public bool RemoveMob(Mob mob)
        {
            return _mobs.Remove(mob);
        }

        public int MobCount {get { return _mobs.Count; }}

        public IEnumerable<Mob> Mobs { get { return _mobs; } }

        #endregion

        public override string ToString()
        {
            return string.Format("Tile {0},{1}", Row, Column);
        }
    }
}
