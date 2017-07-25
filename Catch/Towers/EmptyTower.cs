using Catch.Map;

namespace Catch.Towers
{
    /// <summary>
    /// This tower is empty, but provides Commands for placing new towers.
    /// </summary>
    public class EmptyTower : TowerBase
    {
        public EmptyTower(EmptyTowerSharedResources resources, Tile tile) : base(nameof(EmptyTower), tile)
        {
            Brain = resources.Brain;
            Indicators.AddRange(resources.Indicators);

            var labelText = string.Format("{0},{1}", tile.Coords.Q, tile.Coords.R);
            Indicators.Add(resources.GetLabel(labelText));

            Commands.Add(new BuyTowerCommand(this));

            DisplayName = "Empty Socket";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }
    }
}
