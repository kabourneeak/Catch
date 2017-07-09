using Windows.UI;
using Catch.Base;
using Catch.Map;

namespace Catch.Towers
{
    public class GunTower : TowerBase, ITileAgent
    {
        public override string GetAgentType() => nameof(GunTower);

        public GunTower(GunTowerSharedResources resources, Tile tile, ILevelStateModel level) : base(tile, level)
        {
            Modifiers.Add(new GunTowerBaseModifier(this));

            Brain = new GunTowerBaseBehaviour(this, Level.Config);

            Indicators.AddRange(resources.Indicators);

            DisplayName = "Gun Tower";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }
    }
}
