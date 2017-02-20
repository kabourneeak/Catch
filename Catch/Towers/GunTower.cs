using Windows.UI;
using Catch.Map;

namespace Catch.Towers
{
    public class GunTower : TowerBase
    {
        public GunTower(Tile tile, ILevelStateModel level) : base(tile, level)
        {
            Modifiers.Add(new GunTowerBaseModifier(this));

            Brain = new GunTowerBaseBehaviour(this, Level.Config);

            Indicators.Add(new TowerTileIndicator(Level.Config, Colors.DeepSkyBlue));
            Indicators.Add(new GunTowerBaseIndicator(Level.Config));

            DisplayName = "Gun Tower";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }

        #region IGameObject Implementation

        // no overrides

        #endregion

        #region IAgent Implementation

        public override string GetAgentType()
        {
            return nameof(GunTower);
        }
        
        #endregion
    }
}
