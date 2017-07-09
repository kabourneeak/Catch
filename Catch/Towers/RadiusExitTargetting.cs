using System;
using System.Collections.Generic;
using Catch.Map;
using Catch.Mobs;
using CatchLibrary;

namespace Catch.Towers
{
    /// <summary>
    /// Locates the best targets for a given tile, within its targetting radius, with
    /// priority given to targets closed to an end of a path.
    /// </summary>
    public class RadiusExitTargetting : TargettingBase
    {
        private readonly List<Tuple<int, Tile>> _ranked;

        public RadiusExitTargetting(Tile center, int fromRadius, int toRadius)
        {
            _ranked = new List<Tuple<int, Tile>>();
            
            var neighbours = center.Map.GetNeighbours(center, fromRadius, toRadius);

            if (neighbours.Count == 0)
                throw new ArgumentException(string.Format("The specific tile {0} has no neighbours.", center));

            RankNeighbours(neighbours, center.Map);
        }

        private void RankNeighbours(List<Tile> neighbours, MapModel map)
        {
            foreach (var tile in neighbours)
            {
                var score = CalcExitScore(tile, map);
                _ranked.Add(new Tuple<int, Tile>(score, tile));
            }

            _ranked.Sort(RadiusExitTargettingComparer.GetComparer());
        }

        private int CalcExitScore(Tile tile, MapModel map)
        {
            var bestScore = Int32.MaxValue;

            // check tile against every path, and see how far from the end the tile is
            foreach (var p in map.Paths)
            {
                var index = p.IndexOf(tile);

                var distFromEnd = p.Count - index;
                bestScore = Math.Min(distFromEnd, bestScore);
            }

            // a lower score is higher-priority
            return bestScore;
        }

        public override Tile GetBestTargetTile()
        {
            Tile tile = null;

            foreach (var tuple in _ranked)
            {
                tile = tuple.Item2;

                if (tile.MobCount > 0)
                    break;
            }

            DebugUtils.Assert(tile != null);

            return tile;
        }

        public override MobBase GetBestTargetMob()
        {
            return GetBestTargetMob(GetBestTargetTile());
        }

        public override MobBase GetBestTargetMob(Tile tile)
        {
            MobBase best = null;

            foreach (var mob in tile.Mobs)
            {
                if (best == null)
                    best = mob;
                else if (best.TileProgress < mob.TileProgress && mob.IsTargetable)
                    best = mob;
            }

            return best;
        }

        private class RadiusExitTargettingComparer : IComparer<Tuple<int, Tile>>
        {
            private static RadiusExitTargettingComparer _instance;

            public static RadiusExitTargettingComparer GetComparer()
            {
                return _instance ?? (_instance = new RadiusExitTargettingComparer());
            }

            private RadiusExitTargettingComparer()
            {

            }

            public int Compare(Tuple<int, Tile> x, Tuple<int, Tile> y)
            {
                return x.Item1.CompareTo(y.Item1);
            }
        }
    }
}
