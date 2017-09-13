﻿using Catch.Base;
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
            var agent = new BlockMob(_resources, args.Path);
            agent.Tile = args.Tile;
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
