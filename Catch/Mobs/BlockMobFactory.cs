using Catch.Base;
using Catch.Graphics;

namespace Catch.Mobs
{
    public class BlockMobFactory : IAgentFactory
    {
        private readonly BlockMobSharedResources _resources;

        public string AgentType => nameof(BlockMob);

        public BlockMobFactory()
        {
            _resources = new BlockMobSharedResources();
        }

        public IAgent CreateAgent(CreateAgentArgs args)
        {
            return new BlockMob(_resources, args.Path, args.StateModel);
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
