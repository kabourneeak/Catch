using System;
using System.Collections.Generic;
using System.Linq;
using Catch.Base;

namespace Catch.Towers
{
    /// <summary>
    /// Locates the best targets for a given tile, within its targetting radius, with
    /// priority given to targets closed to an end of a path.
    /// </summary>
    public class RadiusExitTargetting : TargettingBase
    {
        private static readonly IComparer<Tuple<int, IMapTile>> RankComparer =
            Comparer<Tuple<int, IMapTile>>.Create((t1, t2) => t1.Item1.CompareTo(t2.Item1));

        private readonly IMapTile[] _ranked;
        private readonly int[] _agentVersions;
        private readonly int[] _tileAgentVersions;

        public RadiusExitTargetting(IMap map, IMapTile center, int fromRadius, int toRadius)
        {
            var neighbours = map.GetNeighbours(center, fromRadius, toRadius);

            var ranked = RankNeighbours(neighbours, map);

            _ranked = ranked.Select(t => t.Item2).ToArray();
            _agentVersions = _ranked.Select(t => t.AgentVersion).ToArray();
            _tileAgentVersions = _ranked.Select(t => t.TileAgentVersion).ToArray();
        }

        private IEnumerable<Tuple<int, IMapTile>> RankNeighbours(IEnumerable<IMapTile> neighbours, IMap map)
        {
            var ranked = new List<Tuple<int, IMapTile>>();

            foreach (var tile in neighbours)
            {
                var score = CalcExitScore(tile, map);
                ranked.Add(new Tuple<int, IMapTile>(score, tile));
            }

            ranked.Sort(RankComparer);

            return ranked;
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
            foreach (var tile in _ranked)
            {
                if (tile.Agents.Any(a => a != tile.TileAgent))
                    return tile;
            }

            return _ranked.First();
        }

        public override IAgent GetBestTargetMob(IMapTile tile)
        {
            IAgent bestTarget = null;

            var filteredAgents = tile.Agents
                .Where(a => a != tile.TileAgent)
                .Where(a => a.Stats.Team != OwnTeam);

            foreach (var potentialTarget in filteredAgents)
            {
                if (bestTarget == null)
                    bestTarget = potentialTarget;
                else if (bestTarget.TileProgress < potentialTarget.TileProgress)
                    bestTarget = potentialTarget;
            }

            return bestTarget;
        }

        public override int GetAgentVersionDelta()
        {
            var delta = 0;

            for (var i = 0; i < _ranked.Length; ++i)
            {
                var curVersion = _ranked[i].AgentVersion;
                delta += curVersion - _agentVersions[i];
                _agentVersions[i] = curVersion;
            }

            return delta;
        }

        public override int GetTileAgentVersionDelta()
        {
            var delta = 0;

            for (var i = 0; i < _ranked.Length; ++i)
            {
                var curVersion = _ranked[i].TileAgentVersion;
                delta += curVersion - _tileAgentVersions[i];
                _tileAgentVersions[i] = curVersion;
            }

            return delta;
        }
    }
}
