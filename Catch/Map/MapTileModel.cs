using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Services;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    public class MapTileModel : IMapTile
    {
        private readonly ISet<IAgent> _agents;

        public MapTileModel(HexCoords coords, IConfig config)
        {
            Coords = coords;

            _agents = new HashSet<IAgent>();

            // copy down config
            var radius = config.GetFloat("TileRadius");

            // calculate position
            var radiusH = HexUtils.GetRadiusHeight(radius);

            var x = Coords.Column * (radius + radius * HexUtils.COS60);
            var y = ((Coords.Column & 1) * radiusH) + ((Coords.Row - (Coords.Column & 1)) * 2 * radiusH);

            Position = new Vector2(x, y);
        }

        public HexCoords Coords { get; }

        public Vector2 Position { get; }

        #region Agent Management 

        public ITileAgent TileAgent { get; set; }

        public bool AddAgent(IAgent agent) => _agents.Add(agent);

        public bool RemoveAgent(IAgent agent) => _agents.Remove(agent);

        public int AgentCount => _agents.Count;

        public IEnumerable<IAgent> Agents => _agents;

        #endregion

        public override string ToString() => string.Format(nameof(MapTileModel) + " %s", Coords);
    }
}
