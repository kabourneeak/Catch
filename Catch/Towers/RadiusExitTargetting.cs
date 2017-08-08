using System;
using System.Collections.Generic;
using System.Linq;
using Catch.Base;
using CatchLibrary;

namespace Catch.Towers
{
    /// <summary>
    /// Locates the best targets for a given tile, within its targetting radius, with
    /// priority given to targets closed to an end of a path.
    /// </summary>
    public class RadiusExitTargetting : TargettingBase
    {
        private readonly List<Tuple<int, IMapTile>> _ranked;

        public RadiusExitTargetting()
        {
            _ranked = new List<Tuple<int, IMapTile>>();
        }

        public void Initialize(IMap map, IMapTile center, int fromRadius, int toRadius)
        {
            _ranked.Clear();

            var neighbours = map.GetNeighbours(center, fromRadius, toRadius);

            RankNeighbours(neighbours, map);
        }

        private void RankNeighbours(IEnumerable<IMapTile> neighbours, IMap map)
        {
            foreach (var tile in neighbours)
            {
                var score = CalcExitScore(tile, map);
                _ranked.Add(new Tuple<int, IMapTile>(score, tile));
            }

            _ranked.Sort(RadiusExitTargettingComparer.GetComparer());
        }

        private int CalcExitScore(IMapTile tile, IMap map)
        {
            var bestScore = int.MaxValue;

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

        public override IMapTile GetBestTargetTile()
        {
            IMapTile tile = null;

            foreach (var tuple in _ranked)
            {
                tile = tuple.Item2;

                // BUG this is counting the TileAgent
                if (tile.AgentCount > 0)
                    break;
            }

            DebugUtils.Assert(tile != null);

            return tile;
        }

        public override IAgent GetBestTargetMob()
        {
            return GetBestTargetMob(GetBestTargetTile());
        }

        public override IAgent GetBestTargetMob(IMapTile tile)
        {
            IAgent best = null;

            // TODO filter by team?
            foreach (var mob in tile.Agents.Where(a => !(a is ITileAgent)))
            {
                if (best == null)
                    best = mob;
                else if (best.TileProgress < mob.TileProgress)
                    best = mob;
            }

            return best;
        }

        private class RadiusExitTargettingComparer : IComparer<Tuple<int, IMapTile>>
        {
            private static RadiusExitTargettingComparer _instance;

            public static RadiusExitTargettingComparer GetComparer()
            {
                return _instance ?? (_instance = new RadiusExitTargettingComparer());
            }

            private RadiusExitTargettingComparer()
            {

            }

            public int Compare(Tuple<int, IMapTile> x, Tuple<int, IMapTile> y)
            {
                return x.Item1.CompareTo(y.Item1);
            }
        }
    }
}
