using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Mobs;
using Catch.Services;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    public class Tile
    {
        private readonly ISet<MobBase> _mobs;

        public Tile(HexCoords coords, MapModel map, IConfig config)
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

        public MapModel Map { get; }

        public HexCoords Coords { get; }

        public Vector2 Position { get; }

        public ITileAgent TileAgent { get; set; }

        #region Mob Management 

        public bool AddMob(MobBase mob) => _mobs.Add(mob);

        public bool ContainsMob(MobBase mob) => _mobs.Contains(mob);

        public bool RemoveMob(MobBase mob) => _mobs.Remove(mob);

        public int MobCount => _mobs.Count;

        public IEnumerable<MobBase> Mobs => _mobs;

        #endregion

        public override string ToString() => string.Format(nameof(Tile) + " %s", Coords);
    }
}
