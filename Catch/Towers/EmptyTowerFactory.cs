using Catch.Base;
using Catch.Graphics;

namespace Catch.Towers
{
    public class EmptyTowerFactory : IAgentFactory
    {
        private readonly EmptyTowerGraphicsProvider _resources;

        public string AgentType => nameof(EmptyTower);

        public EmptyTowerFactory(IGraphicsManager graphicsManager)
        {
            _resources = graphicsManager.Resolve<EmptyTowerGraphicsProvider>();
        }

        public IExtendedAgent CreateAgent(CreateAgentArgs args)
        {
            var agent = new EmptyTower(_resources, args.Tile);
            agent.ExtendedStats.Team = args.Team;

            return agent;
        }
    }
}
