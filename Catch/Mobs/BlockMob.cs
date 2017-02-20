using Windows.UI;
using Catch.Graphics;
using Catch.Map;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Mobs
{
    public class BlockMob : MobBase
    {
        public BlockMob(MapPath mapPath, ILevelStateModel level) : base(level)
        {
            // This class can be massively generalized!  e.g., take in a config object and a mob name, and then fill out
            // all of the other details like health, speed, attack, defense, mod loadout, from config.

            // TODO copy down relevant config

            int blockSize = 20;
            float velocity = 0.005f;

            Brain = new PathMobBehaviour(this, mapPath, velocity);

            var strokeStyle = new CanvasStrokeStyle() {LineJoin = CanvasLineJoin.Round};
            var style = new StyleArgs() { BrushType = BrushType.Solid, Color = Colors.Yellow, StrokeWidth = 4, StrokeStyle = strokeStyle};
            Indicators.Add(new BlockMobBaseIndicator(blockSize, style));
        }

        public override string GetAgentType()
        {
            return nameof(BlockMob);
        }
    }
}
