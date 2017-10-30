using System;
using Catch.Base;
using Catch.Graphics;
using Catch.Mobs;
using Catch.Services;
using Catch.Towers;
using Unity;

namespace Catch.Level
{
    /// <summary>
    /// Provides agents built into the program
    /// </summary>
    public class BuiltinAgentProvider : IAgentProvider
    {
        private readonly IConfig _config;
        private readonly IUnityContainer _container;

        public BuiltinAgentProvider(IConfig config, IUnityContainer container)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public IExtendedAgent CreateAgent(string name, CreateAgentArgs args)
        {
            switch (name)
            {
                case GunTowerBehaviour.AgentTypeName:
                    return CreateGunTowerAgent(args);
                case EmptyTowerBehaviour.AgentTypeName:
                    return CreateEmptyTower(args);
                case BlockMobBehaviour.AgentTypeName:
                    return CreateBlockMob(args);
                default:
                    throw new ArgumentException($"I don't know how to construct an agent with name {name}");
            }
        }

        private IExtendedAgent CreateGunTowerAgent(CreateAgentArgs args)
        {
            var agent = new AgentBase(GunTowerBehaviour.AgentTypeName);
            agent.ExtendedStats.Team = args.Team;
            agent.GraphicsComponent = new RelativePositionGraphicsComponent();
            agent.BehaviourComponent = new GunTowerBehaviour(agent, _config, _container.Resolve<GunTowerGraphicsProvider>(), args.Tile);

            if (agent.BehaviourComponent is IModifier modifier)
                agent.AddModifier(modifier);

            return agent;
        }

        private IExtendedAgent CreateEmptyTower(CreateAgentArgs args)
        {
            var agent = new AgentBase(EmptyTowerBehaviour.AgentTypeName);
            agent.ExtendedStats.Team = args.Team;
            agent.GraphicsComponent = new RelativePositionGraphicsComponent();
            agent.BehaviourComponent = new EmptyTowerBehaviour(agent, _container.Resolve<EmptyTowerGraphicsProvider>(), args.Tile);

            if (agent.BehaviourComponent is IModifier modifier)
                agent.AddModifier(modifier);

            return agent;
        }

        private IExtendedAgent CreateBlockMob(CreateAgentArgs args)
        {
            var agent = new AgentBase(BlockMobBehaviour.AgentTypeName);
            agent.Tile = args.Tile;
            agent.ExtendedStats.Team = args.Team;
            agent.GraphicsComponent = new RelativePositionGraphicsComponent();
            agent.BehaviourComponent = new BlockMobBehaviour(agent, _config, _container.Resolve<BlockMobGraphicsProvider>(), args.Path);

            if (agent.BehaviourComponent is IModifier modifier)
                agent.AddModifier(modifier);

            return agent;
        }
    }
}
