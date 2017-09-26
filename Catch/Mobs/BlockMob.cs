using Catch.Base;
using Catch.Services;

namespace Catch.Mobs
{
    public class BlockMob : AgentBase
    {
        public static readonly string CfgBlockSize = ConfigUtils.GetConfigPath(nameof(BlockMob), nameof(CfgBlockSize));
        public static readonly string CfgVelocity = ConfigUtils.GetConfigPath(nameof(BlockMob), nameof(CfgVelocity));

        public BlockMob(IConfig config, BlockMobSharedResources resources, IMapPath mapPath) : base(nameof(BlockMob))
        {
            // This class can be massively generalized!  e.g., take in a config object and a mob name, and then fill out
            // all of the other details like health, speed, attack, defense, mod loadout, from config.

            var velocity = config.GetFloat(CfgVelocity);
            Brain = new PathMobBehaviour(this, mapPath, velocity);

            Indicators.AddRange(resources.Indicators);
        }
    }
}
