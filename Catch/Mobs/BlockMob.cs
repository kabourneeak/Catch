using Windows.UI;
using Catch.Graphics;
using Catch.Map;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Mobs
{
    public class BlockMob : MobBase
    {
        public BlockMob(BlockMobSharedResources resources, MapPath mapPath) : base(nameof(BlockMob))
        {
            // This class can be massively generalized!  e.g., take in a config object and a mob name, and then fill out
            // all of the other details like health, speed, attack, defense, mod loadout, from config.

            // TODO copy down relevant config

            float velocity = 0.005f;
            Brain = new PathMobBehaviour(this, mapPath, velocity);

            Indicators.AddRange(resources.Indicators);
        }
    }
}
