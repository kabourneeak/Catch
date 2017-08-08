using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    public class MapTileModel : IMapTile
    {
        private readonly ISet<IAgent> _agents;
        private readonly ISet<IDrawable> _drawables;

        public MapTileModel(HexCoords coords, IConfig config)
        {
            Coords = coords;

            _agents = new HashSet<IAgent>();
            _drawables = new HashSet<IDrawable>();

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

        public bool AddAgent(IAgent agent)
        {
            var added = _agents.Add(agent);
            if (agent is IDrawable drawable)
                _drawables.Add(drawable);

            return added;
        }

        public bool RemoveAgent(IAgent agent)
        {
            var removed = _agents.Remove(agent);
            if (agent is IDrawable drawable)
                _drawables.Remove(drawable);

            return removed;
        }

        public int AgentCount => _agents.Count;

        public IEnumerable<IAgent> Agents => _agents;

        public IEnumerable<IDrawable> Drawables => _drawables;

        #endregion

        public override string ToString() => string.Format(nameof(MapTileModel) + " %s", Coords);
    }
}
