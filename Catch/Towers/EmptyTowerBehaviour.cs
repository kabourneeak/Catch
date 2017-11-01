using Catch.Base;
using Catch.Graphics;
using Catch.Level;

namespace Catch.Towers
{
    /// <summary>
    /// This tower is empty, but provides Commands for placing new towers.
    /// </summary>
    public class EmptyTowerBehaviour : NilAgentBehaviour, IAgentStatsModifier
    {
        public const string AgentTypeName = "EmptyTower";

        public EmptyTowerBehaviour(IExtendedAgent host, IndicatorProvider indicatorProvider)
        {
            host.Position = host.Tile.Position;

            var labelIndicator = (TextIndicator) indicatorProvider.GetIndicator("EmptyTileLabel");
            var labelText = string.Format("{0},{1}", host.Tile.Coords.Q, host.Tile.Coords.R);
            labelIndicator.SetLabelText(labelText);
            host.Indicators.Add(labelIndicator);

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
