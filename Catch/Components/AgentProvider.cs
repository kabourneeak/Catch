using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Graphics;
using Catch.Mobs;
using Catch.Services;
using Catch.Towers;
using CatchLibrary.Serialization.Assets;
using Unity;

namespace Catch.Components
{
    /// <summary>
    /// Provides agents built into the program
    /// </summary>
    public class AgentProvider : IProvider, IAgentProvider
    {
        private readonly IndicatorProvider _indicatorProvider;
        private readonly ModifierProvider _modifierProvider;
        private readonly CommandProvider _commandProvider;
        private readonly BehaviourProvider _behaviourProvider;
        private readonly IUnityContainer _container;

        private readonly Dictionary<string, AgentModel> _models;

        public AgentProvider(AssetModel assetModel,
            IndicatorProvider indicatorProvider,
            ModifierProvider modifierProvider,
            CommandProvider commandProvider,
            BehaviourProvider behaviourProvider,
            IUnityContainer container)
        {
            _indicatorProvider = indicatorProvider;
            _modifierProvider = modifierProvider;
            _commandProvider = commandProvider;
            _behaviourProvider = behaviourProvider;
            _container = container ?? throw new ArgumentNullException(nameof(container));

            _models = new Dictionary<string, AgentModel>();

            foreach (var model in assetModel.Agents)
            {
                _models.Add(model.Name, model);
            }
        }

        public IExtendedAgent CreateAgent(string name, CreateAgentArgs args)
        {
            // Create a child container for specific, agent-scoped dependencies to be used
            // during the construction of this single agent
            var agentContainer = _container.CreateChildContainer();

            var agent = new AgentBase(name);

            agentContainer.RegisterInstance<IAgent>(agent);
            agentContainer.RegisterInstance<IExtendedAgent>(agent);


            // TODO this will not be scoped to the other providers
            if (args.Path != null)
                agentContainer.RegisterInstance<IMapPath>(args.Path);

            // assemble agent
            if (_models.TryGetValue(name, out var model))
            {
                agent.Tile = args.Tile;
                agent.ExtendedStats.Team = args.Team;

                agent.GraphicsComponent = agentContainer.Resolve<RelativePositionGraphicsComponent>();

                foreach (var indicatorName in model.IndicatorNames)
                    agent.Indicators.Add(_indicatorProvider.GetIndicator(indicatorName, agent));

                foreach (var modifierName in model.ModifierNames)
                    agent.AddModifier(_modifierProvider.GetModifier(modifierName, agent));

                foreach (var commandName in model.CommandNames)
                    agent.CommandCollection.Add(_commandProvider.GetCommand(commandName, agent));

                // add behaviours
                agent.BehaviourComponent = _behaviourProvider.GetBehaviour(model.PrimaryBehaviourName, agent);

                if (agent.BehaviourComponent is IModifier modifier)
                    agent.AddModifier(modifier);

                return agent;
            }

            throw new ArgumentException($"The agent {name} is not defined");
        }
    }
}
