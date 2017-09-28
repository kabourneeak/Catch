using Catch.Base;

namespace Catch.Towers
{
    public class GunTower : TowerBase, IExtendedTileAgent
    {
        public GunTower(GunTowerSharedResources resources, IMapTile tile) : base(nameof(GunTower), tile)
        {
            // TODO how much of this can we push to the factory?

            BaseModifierCollection.Add(new GunTowerBaseModifier());

            Brain = new GunTowerBaseBehaviour(this, resources.Config);

            Indicators.AddRange(resources.Indicators);

            DisplayName = "Gun Tower";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }
    }
}
