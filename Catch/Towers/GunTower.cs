using Catch.Base;
using Catch.Map;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTower : TowerBase, ITileAgent
    {
        public GunTower(GunTowerSharedResources resources, Tile tile) : base(nameof(GunTower), tile)
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
