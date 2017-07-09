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
            return new EmptyTower(_resources, args.Tile, args.StateModel);
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
