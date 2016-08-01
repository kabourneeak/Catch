﻿using Windows.UI;
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

            Layer = DrawLayer.Tower;

            Modifiers.Add(new GunTowerBaseModifier(this));

            Brain = new GunTowerBaseBehaviour(this, config);

            var radius = config.GetFloat("TileRadius");
            var inset = config.GetFloat("TileRadiusInset");
            var style = new StyleArgs() { BrushType = BrushType.Solid, Color = Colors.DeepSkyBlue, StrokeWidth = 3};

            Indicators.Add(new HexagonGraphics(radius - inset, style));
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