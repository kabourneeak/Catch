﻿using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Map
{
    public class TileAreaIndicator : HexagonIndicator, IIndicator
    {
        public TileAreaIndicator(IConfig config, Color color)
        {
            var radius = config.GetFloat(CoreConfig.TileRadius);
            var inset = config.GetFloat(CoreConfig.TileRadiusInset);

            Radius = radius - inset;
            Style = new StyleArgs() { BrushType = BrushType.Solid, Color = color, StrokeWidth = 3 };
            Layer = DrawLayer.Base;
            Filled = true;
            LevelOfDetail = DrawLevelOfDetail.All;
        }
    }
}