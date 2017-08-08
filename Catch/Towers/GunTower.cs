using Catch.Base;

namespace Catch.Towers
{
    public class GunTower : TowerBase, ITileAgent
    {
        public GunTower(GunTowerSharedResources resources, IMapTile tile) : base(nameof(GunTower), tile)
        {
            Modifiers.Add(new GunTowerBaseModifier(this));

            Brain = new GunTowerBaseBehaviour(this, resources.Config);

            Indicators.AddRange(resources.Indicators);

            DisplayName = "Gun Tower";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }
    }
}
