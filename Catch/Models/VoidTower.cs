using System;
using Windows.UI;
using Catch.Base;
using Catch.Services;

namespace Catch.Models
{
    /// <summary>
    /// This tower has no behaviour nor interactions, but draws a background hexagon. This tower
    /// gives visual definition to a map, and may be part of a MapPath.
    /// </summary>
    public class VoidTower : Tower
    {
        public VoidTower(Tile tile, IConfig config) : base(tile)
        {
            // copy down config
            var radius = config.GetFloat("TileRadius");
            var style = new StyleArgs() { BrushType = BrushType.Solid, Color = Colors.DarkRed, StrokeWidth = 3, BrushOpacity = 0.5f };

            Brain = new EmptyBrain();
            Indicators.Add(new HexagonGraphics(tile.Position, radius, style));
        }

        public override string GetAgentType()
        {
            return typeof(NilTower).Name;
        }
    }
}
