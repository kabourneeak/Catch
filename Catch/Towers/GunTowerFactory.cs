using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerFactory : IAgentFactory
    {
        private readonly GunTowerSharedResources _resources;

        public string AgentType => nameof(GunTower);

        public GunTowerFactory(IConfig config)
        {
            _resources = new GunTowerSharedResources(config);
        }

        public IAgent CreateAgent(CreateAgentArgs args)
        {
            var agent = new GunTower(_resources, args.Tile);
            agent.Stats.Team = args.Team;

            return agent;
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            _resources.CreateResources(args);
        }

        public void DestroyResources()
        {
            _resources.DestroyResources();
        }
    }
}
