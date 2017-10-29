using Catch.Base;
using Catch.Graphics;

namespace Catch.Towers
{
    public class GunTowerFactory : IAgentFactory
    {
        private readonly GunTowerGraphicsProvider _resources;

        public string AgentType => nameof(GunTower);

        public GunTowerFactory(IGraphicsManager graphicsManager)
        {
            _resources = graphicsManager.Resolve<GunTowerGraphicsProvider>();
        }

        public IExtendedAgent CreateAgent(CreateAgentArgs args)
        {
            var agent = new GunTower(_resources, args.Tile);
            agent.ExtendedStats.Team = args.Team;

            return agent;
        }
    }
}
