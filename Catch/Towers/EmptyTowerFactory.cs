using Catch.Base;
using Catch.Graphics;

namespace Catch.Towers
{
    public class EmptyTowerFactory : IAgentFactory
    {
        private readonly EmptyTowerSharedResources _resources;

        public string AgentType => nameof(EmptyTower);

        public EmptyTowerFactory()
        {
            _resources = new EmptyTowerSharedResources();
        }

        public IExtendedAgent CreateAgent(CreateAgentArgs args)
        {
            var agent = new EmptyTower(_resources, args.Tile);
            agent.ExtendedStats.Team = args.Team;

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
