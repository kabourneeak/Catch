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
        private readonly IUnityContainer _providerContainer;

        public BuiltinAgentProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            // Create a child container for agent components
            _providerContainer = container.CreateChildContainer();
            BootstrapProviderContainer();
        }

        public IExtendedAgent CreateAgent(string name, CreateAgentArgs args)
        {
            // Create a child container for specific, agent-scoped dependencies to be used
            // during the construction of this single agent
            var agentContainer = _providerContainer.CreateChildContainer();

            var agent = new AgentBase(name);

            agentContainer.RegisterInstance<IAgent>(agent);
            agentContainer.RegisterInstance<IExtendedAgent>(agent);

            if (args.Path != null)
                agentContainer.RegisterInstance<IMapPath>(args.Path);            

            // configure agent
            switch (name)
            {
                case GunTowerBehaviour.AgentTypeName:
                    return CreateGunTowerAgent(agent, agentContainer, args);
                case EmptyTowerBehaviour.AgentTypeName:
                    return CreateEmptyTower(agent, agentContainer, args);
                case BlockMobBehaviour.AgentTypeName:
                    return CreateBlockMob(agent, agentContainer, args);
                default:
                    throw new ArgumentException($"I don't know how to construct an agent with name {name}");
            }
        }

        private IExtendedAgent CreateGunTowerAgent(AgentBase agent, IUnityContainer container, CreateAgentArgs args)
        {
            agent.Tile = args.Tile;
            agent.ExtendedStats.Team = args.Team;
            agent.GraphicsComponent = container.Resolve<RelativePositionGraphicsComponent>();
            agent.BehaviourComponent = container.Resolve<GunTowerBehaviour>();

            if (agent.BehaviourComponent is IModifier modifier)
                agent.AddModifier(modifier);

            return agent;
        }

        private IExtendedAgent CreateEmptyTower(AgentBase agent, IUnityContainer container, CreateAgentArgs args)
        {
            agent.Tile = args.Tile;
            agent.ExtendedStats.Team = args.Team;
            agent.GraphicsComponent = container.Resolve<RelativePositionGraphicsComponent>();
            agent.BehaviourComponent = container.Resolve<EmptyTowerBehaviour>();

            if (agent.BehaviourComponent is IModifier modifier)
                agent.AddModifier(modifier);

            return agent;
        }

        private IExtendedAgent CreateBlockMob(AgentBase agent, IUnityContainer container, CreateAgentArgs args)
        {
            agent.Tile = args.Tile;
            agent.ExtendedStats.Team = args.Team;
            agent.GraphicsComponent = container.Resolve<RelativePositionGraphicsComponent>();
            agent.BehaviourComponent = container.Resolve<BlockMobBehaviour>();

            if (agent.BehaviourComponent is IModifier modifier)
                agent.AddModifier(modifier);

            return agent;
        }

        private void BootstrapProviderContainer()
        {
            UnityUtils.RegisterAllAsTransient(typeof(IGraphicsComponent), _providerContainer);
            UnityUtils.RegisterAllAsTransient(typeof(IModifier), _providerContainer);
            UnityUtils.RegisterAllAsTransient(typeof(IUpdatable), _providerContainer);
        }
    }
}
