using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class TowerHoverIndicator : IIndicator
    {
        private readonly HexagonGraphics _graphics;

        public TowerHoverIndicator(IConfig config)
        {
            var radius = config.GetFloat("TileRadius");
            var inset = config.GetFloat("TileRadiusInset");
            var style = new StyleArgs() { BrushType = BrushType.Solid, Color = Colors.Yellow, StrokeWidth = 3 };

            _graphics = new HexagonGraphics(radius - inset, style);
        }

        public void Update(float ticks)
        {
            // do nothing
        }

        public void CreateResources(CreateResourcesArgs createArgs) => _graphics.CreateResources(createArgs);

        public void DestroyResources() => _graphics.DestroyResources();

        public void Draw(DrawArgs drawArgs, float rotation) => _graphics.Draw(drawArgs, rotation);

        public DrawLayer Layer => DrawLayer.Ui;
    }
}
