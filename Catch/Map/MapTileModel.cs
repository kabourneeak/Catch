using System;
using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Services;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    public class MapTileModel : IMapTile
    {
        private readonly IVersionedCollection<IExtendedAgent> _agents;
        private IExtendedAgent _tileAgent;

        public MapTileModel(HexCoords coords, IConfig config, IndicatorCollection indicatorCollection)
        {
            Coords = coords;

            _agents = new VersionedCollection<IExtendedAgent>(new HashSet<IExtendedAgent>());

            // copy down config
            var radius = config.GetFloat(CoreConfig.TileRadius);

            // calculate position
            var radiusH = HexUtils.GetRadiusHeight(radius);

            var x = Coords.Column * (radius + radius * HexUtils.COS60);
            var y = ((Coords.Column & 1) * radiusH) + ((Coords.Row - (Coords.Column & 1)) * 2 * radiusH);

            Position = new Vector2(x, y);
            Indicators = indicatorCollection;
        }

        public HexCoords Coords { get; }

        public Vector2 Position { get; }

        public IndicatorCollection Indicators { get; }

        #region Agent Management 

        /// <summary>
        /// Gets or sets the current TileAgent for this map tile. 
        /// Throws ArgumentException if the agent being set at the current TileAgent is not in the <see cref="Agents"/> collection
        /// </summary>
        public IAgent TileAgent => _tileAgent;

        public IExtendedAgent ExtendedTileAgent => _tileAgent;

        public void SetTileAgent(IExtendedAgent agent)
        {
            if (ReferenceEquals(_tileAgent, agent))
                return;

            if (agent != null && !_agents.Contains(agent))
                throw new ArgumentException("Cannot set TileAgent to agent not present in Agents collection");

            _tileAgent = agent;

            TileAgentVersion += 1;
        }

        public void AddAgent(IExtendedAgent agent) => _agents.Add(agent);

        public bool RemoveAgent(IExtendedAgent agent) => _agents.Remove(agent);

        public int AgentCount => _agents.Count;

        public IVersionedEnumerable<IAgent> Agents => _agents;

        public IVersionedEnumerable<IExtendedAgent> ExtendedAgents => _agents;

        public int AgentVersion => _agents.Version;

        public int TileAgentVersion { get; private set; }

        #endregion

        public override string ToString() => $"{nameof(MapTileModel)} {Coords}";
    }
}
