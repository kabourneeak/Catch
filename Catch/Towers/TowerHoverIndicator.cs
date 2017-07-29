﻿using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class TowerHoverIndicator : HexagonIndicator, IIndicator
    {
        public TowerHoverIndicator(IConfig config)
        {
            var radius = config.GetFloat("TileRadius");
            var inset = config.GetFloat("TileRadiusInset");

            Radius = radius - inset;
            Style = new StyleArgs() { BrushType = BrushType.Solid, Color = Colors.Yellow, StrokeWidth = 3 };
            Layer = DrawLayer.Ui;
            LevelOfDetail = DrawLevelOfDetail.All;
        }
    }
}
