using Catch.Base;
using Catch.Map;

namespace Catch.Towers
{
    public class GunTower : TowerBase, ITileAgent
    {
        public GunTower(GunTowerSharedResources resources, Tile tile, ILevelStateModel level) : base(nameof(GunTower), tile, level)
        {
            Modifiers.Add(new GunTowerBaseModifier(this));

            Brain = new GunTowerBaseBehaviour(this, level.Config);

            Indicators.AddRange(resources.Indicators);

            DisplayName = "Gun Tower";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }
    }
}
