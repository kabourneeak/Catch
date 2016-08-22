using Windows.UI;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTower : TowerBase
    {
        private readonly IConfig _config;

        public GunTower(Tile tile, IConfig config) : base(tile)
        {
            _config = config;

            Modifiers.Add(new GunTowerBaseModifier(this));

            Brain = new GunTowerBaseBehaviour(this, config);

            Indicators.Add(new TowerTileIndicator(config, Colors.DeepSkyBlue));
            Indicators.Add(new GunTowerBaseIndicator(config));
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
