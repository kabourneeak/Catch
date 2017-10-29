using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Mobs
{
    public class BlockMobFactory : IAgentFactory
    {
        private readonly IConfig _config;
        private readonly BlockMobGraphicsProvider _resources;

        public string AgentType => nameof(BlockMob);

        public BlockMobFactory(IConfig config, IGraphicsManager graphicsManager)
        {
            _config = config;
            _resources = graphicsManager.Resolve<BlockMobGraphicsProvider>();
        }

        public IExtendedAgent CreateAgent(CreateAgentArgs args)
        {
            var agent = new BlockMob(_config, _resources, args.Path);
            agent.Tile = args.Tile;
            agent.ExtendedStats.Team = args.Team;

            return agent;
        }
    }
}
