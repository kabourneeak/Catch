using Catch.Base;

namespace Catch.Towers
{
    /// <summary>
    /// This tower is empty, but provides Commands for placing new towers.
    /// </summary>
    public class EmptyTower : TowerBase
    {
        public EmptyTower(EmptyTowerSharedResources resources, IMapTile tile) : base(nameof(EmptyTower), tile)
        {
            Indicators.AddRange(resources.Indicators);

            var labelText = string.Format("{0},{1}", tile.Coords.Q, tile.Coords.R);
            Indicators.Add(resources.GetLabel(labelText));

            CommandCollection.Add(new BuyTowerCommand(this));

            DisplayName = "Empty Socket";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }

        public override float Update(IUpdateEventArgs args)
        {
            // indicate that we don't need anymore updates
            return 0.0f;
        }
    }
}
