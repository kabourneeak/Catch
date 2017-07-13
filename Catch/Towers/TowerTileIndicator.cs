using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    /// <summary>
    /// A generic tower indicator which draws the hexagonal tile border
    /// that the tower occupies.
    /// </summary>
    public class TowerTileIndicator : HexagonIndicator, IIndicator
    {
        public TowerTileIndicator(IConfig config, Color color)
        {
            var radius = config.GetFloat("TileRadius");
            var inset = config.GetFloat("TileRadiusInset");

            Radius = radius - inset;
            Style = new StyleArgs() { BrushType = BrushType.Solid, Color = color, StrokeWidth = 3 };
            Layer = DrawLayer.Base;
        }
    }
}
