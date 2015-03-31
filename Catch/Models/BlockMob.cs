using Catch.Base;

namespace Catch.Models
{
    public class BlockMob : PathMobAgent
    {
        public BlockMob(IPath path, IBehaviourComponent brain, IGraphicsComponent graphics) : base(path, brain, graphics)
        {
            // This class can be massively generalized!  e.g., take in a config object and a mob name, and then fill out
            // all of the other details like health, speed, attack, defense, mod loadout, from config.
        }

        public override string GetAgentType()
        {
            return typeof (BlockMob).Name;
        }
    }
}
