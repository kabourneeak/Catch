using Catch.Base;

namespace Catch.Towers
{
    /// <summary>
    /// This tower is empty, but provides Commands for placing new towers.
    /// </summary>
    public class EmptyTowerBehaviour : NilAgentBehaviour, IAgentStatsModifier
    {
        public const string AgentTypeName = "EmptyTower";

        public EmptyTowerBehaviour(IExtendedAgent host, EmptyTowerGraphicsProvider resources, IMapTile tile)
        {
            host.Tile = tile;
            host.Position = tile.Position;

            var labelText = string.Format("{0},{1}", tile.Coords.Q, tile.Coords.R);
            host.Indicators.Add(resources.GetLabel(labelText));

            host.CommandCollection.Add(new BuyTowerCommand(host));
        }

        public ModifierPriority Priority => ModifierPriority.Base;

        public void OnCalculateAgentStats(IExtendedAgent agent)
        {
            // should come from a modifier
            agent.ExtendedStats.DisplayName = "Empty Socket";
            agent.ExtendedStats.DisplayStatus = string.Empty;
            agent.ExtendedStats.DisplayInfo = string.Empty;
        }
    }
}
