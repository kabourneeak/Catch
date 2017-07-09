using Catch.Map;

namespace Catch.Towers
{
    /// <summary>
    /// This tower is empty, but provides Commands for placing new towers.
    /// </summary>
    public class EmptyTower : TowerBase
    {
        public EmptyTower(EmptyTowerSharedResources resources, Tile tile, ILevelStateModel level) : base(nameof(EmptyTower), tile, level)
        {
            Brain = resources.Brain;
            Indicators.AddRange(resources.Indicators);

            var labelText = string.Format("{0},{1}", tile.Coords.Q, tile.Coords.R);
            Indicators.Add(resources.GetLabel(labelText));

            // TODO fix this
            //Commands.Add(new BuyTowerCommand(this, Level));

            DisplayName = "Empty Socket";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }
    }
}
