using Windows.UI;
using Catch.Base;
using Catch.Services;

namespace Catch.Models
{
    public class BlockMob : Mob
    {
        private readonly IConfig _config;

        public BlockMob(MapPath mapPath, IConfig config) : base()
        {
            // This class can be massively generalized!  e.g., take in a config object and a mob name, and then fill out
            // all of the other details like health, speed, attack, defense, mod loadout, from config.

            // TODO copy down relevant config
            _config = config;

            int blockSize = 20;
            Color blockColour = Colors.Yellow;
            float velocity = 0.005f;
            

            Brain = new PathMobBehaviour(this, mapPath, velocity);
            Indicators.Add(new BlockMobBaseIndicator(this, blockSize, blockColour));
        }

        public override string GetAgentType()
        {
            return typeof (BlockMob).Name;
        }
    }
}
