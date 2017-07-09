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
    public class TowerTileIndicator : IIndicator
    {
        private readonly HexagonGraphics _graphics;

        public TowerTileIndicator(IConfig config, Color color)
        {
            var radius = config.GetFloat("TileRadius");
            var inset = config.GetFloat("TileRadiusInset");
            var style = new StyleArgs() { BrushType = BrushType.Solid, Color = color, StrokeWidth = 3 };

            _graphics = new HexagonGraphics(radius - inset, style);
        }

        public void Update(float ticks)
        {
            // do nothing
        }

        public void CreateResources(CreateResourcesArgs args) => _graphics.CreateResources(args);

        public void DestroyResources() => _graphics.DestroyResources();

        public void Draw(DrawArgs drawArgs, float rotation) => _graphics.Draw(drawArgs, rotation);

        public DrawLayer Layer => DrawLayer.Base;
    }
}
