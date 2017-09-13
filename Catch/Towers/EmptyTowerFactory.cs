using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class EmptyTowerFactory : IAgentFactory
    {
        private readonly EmptyTowerSharedResources _resources;

        public string AgentType => nameof(EmptyTower);

        public EmptyTowerFactory(IConfig config)
        {
            _resources = new EmptyTowerSharedResources(config);
        }

        public IAgent CreateAgent(CreateAgentArgs args)
        {
            var agent = new EmptyTower(_resources, args.Tile);
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
