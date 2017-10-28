using Catch.Base;

namespace Catch.Towers
{
    /// <summary>
    /// This tower is empty, but provides Commands for placing new towers.
    /// </summary>
    public class EmptyTower : AgentBase
    {
        public EmptyTower(EmptyTowerSharedResources resources, IMapTile tile) : base(nameof(EmptyTower))
        {
            Tile = tile;
            Position = tile.Position;

            Indicators.AddRange(resources.Indicators);

            var labelText = string.Format("{0},{1}", tile.Coords.Q, tile.Coords.R);
            Indicators.Add(resources.GetLabel(labelText));

            CommandCollection.Add(new BuyTowerCommand(this));

            ExtendedStats.DisplayName = "Empty Socket";
            ExtendedStats.DisplayStatus = string.Empty;
            ExtendedStats.DisplayInfo = string.Empty;
        }

        public override float Update(IUpdateEventArgs args)
        {
            // indicate that we don't need anymore updates
            return 0.0f;
        }
    }
}
