using System;
using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Services;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    /// <inheritdoc />
    public class MapTileModel : IMapTile
    {
        private readonly ISet<IAgent> _agents;
        private ITileAgent _tileAgent;

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

        /// <summary>
        /// Gets or sets the current TileAgent for this map tile. 
        /// Throws ArgumentException if the agent being set at the current TileAgent is not in the <see cref="Agents"/> collection
        /// </summary>
        public ITileAgent TileAgent
        {
            get => _tileAgent;
            set
            {
                if (ReferenceEquals(_tileAgent, value))
                    return;

                if (value != null && !_agents.Contains(value))
                    throw new ArgumentException("Cannot set TileAgent to value not present in Agents collection");

                _tileAgent = value;

                TileAgentVersion += 1;
            }
        }

        public bool AddAgent(IAgent agent)
        {
            var wasModified = _agents.Add(agent);

            if (wasModified)
                AgentVersion += 1;

            return wasModified;
        }

        public bool RemoveAgent(IAgent agent)
        {
            var wasModified = _agents.Remove(agent);

            if (wasModified)
                AgentVersion += 1;

            return wasModified;
        }

        public int AgentCount => _agents.Count;

        public IEnumerable<IAgent> Agents => _agents;

        public int AgentVersion { get; private set; }

        public int TileAgentVersion { get; private set; }

        #endregion

        public override string ToString() => string.Format(nameof(MapTileModel) + " %s", Coords);
    }
}
