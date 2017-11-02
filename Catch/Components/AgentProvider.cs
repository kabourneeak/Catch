using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;
using CatchLibrary.Serialization.Assets;
using Unity;

namespace Catch.Components
{
    /// <summary>
    /// Provides agents built into the program
    /// </summary>
    public class AgentProvider : IProvider, IAgentProvider
    {
        private readonly IConfig _parentConfig;
        private readonly IUnityContainer _container;

        private readonly Dictionary<string, AgentModel> _agentModels;
        private readonly Dictionary<string, ComponentModel> _componentModels;
        private readonly Dictionary<string, IConfig> _componentConfigs;

        public AgentProvider(IConfig parentConfig, AssetModel assetModel, IUnityContainer container)
        {
            _parentConfig = parentConfig ?? throw new ArgumentNullException(nameof(parentConfig));
            if (assetModel == null) throw new ArgumentNullException(nameof(assetModel));
            _container = container ?? throw new ArgumentNullException(nameof(container));

            // load models
            _agentModels = new Dictionary<string, AgentModel>();

            foreach (var agentModel in assetModel.Agents)
            {
                _agentModels.Add(agentModel.Name, agentModel);
            }

            _componentModels = new Dictionary<string, ComponentModel>();
            _componentConfigs = new Dictionary<string, IConfig>();

            LoadComponentModels(assetModel.Indicators, parentConfig);
            LoadComponentModels(assetModel.Behaviours, parentConfig);
            LoadComponentModels(assetModel.Commands, parentConfig);
            LoadComponentModels(assetModel.Modifiers, parentConfig);
        }

        private void LoadComponentModels(IEnumerable<ComponentModel> componentModels, IConfig config)
        {
            foreach (var componentModel in componentModels)
            {
                _componentModels.Add(componentModel.Name, componentModel);
                // config entries will be generated the first time they are requested, and blended
                // with their scope.
            }
        }

        public IExtendedAgent CreateAgent(string name, CreateAgentArgs args)
        {
            if (!_agentModels.TryGetValue(name, out var agentModel))
                throw new ArgumentException($"The agent {name} is not defined");

            /*
             * assemble agent
             */

            var agent = new AgentBase(name);

            // Create a child container for specific, agent-scoped dependencies to be used
            // during the construction of this single agent
            // We make a new scope each time incase one of the components keeps a reference
            var agentContainer = _container.CreateChildContainer();

            agentContainer.RegisterInstance<IUnityContainer>(agentContainer);
            agentContainer.RegisterInstance<IAgent>(agent);
            agentContainer.RegisterInstance<IExtendedAgent>(agent);

            if (args.Path != null)
                agentContainer.RegisterInstance<IMapPath>(args.Path);

            agent.Tile = args.Tile;
            agent.ExtendedStats.Team = args.Team;

            agent.GraphicsComponent = agentContainer.Resolve<RelativePositionGraphicsComponent>();

            foreach (var indicatorName in agentModel.IndicatorNames)
                agent.Indicators.Add(GetComponent<IIndicator>(indicatorName, agentContainer));

            foreach (var modifierName in agentModel.ModifierNames)
                agent.AddModifier(GetComponent<IModifier>(modifierName, agentContainer));

            foreach (var commandName in agentModel.CommandNames)
                agent.CommandCollection.Add(GetComponent<IAgentCommand>(commandName, agentContainer));

            // add behaviours
            agent.BehaviourComponent = GetComponent<IUpdatable>(agentModel.PrimaryBehaviourName, agentContainer);

            if (agent.BehaviourComponent is IModifier modifier)
                agent.AddModifier(modifier);

            return agent;
        }

        public T GetComponent<T>(string name, IExtendedAgent agent)
        {
            // TODO consider keeping the original agent scopes in a ConditionalWeakTable 
            // instead of making new scopes here.
            // This would be especially useful if the Agent or the CreateAgentArgs contains
            // information (like config entries) that might be useful later

            if (!_componentModels.TryGetValue(name, out var model))
                throw new ArgumentException($"The component {name} is not defined");

            // Components themselves gain yet another scope, to keep them isolated
            // from multiple components of the same type being used in one agent
            var componentScope = _container.CreateChildContainer();

            componentScope.RegisterInstance<IUnityContainer>(componentScope);
            componentScope.RegisterInstance<IAgent>(agent);
            componentScope.RegisterInstance<IExtendedAgent>(agent);
            componentScope.RegisterInstance<IConfig>(GetConfig(name, _parentConfig));

            return componentScope.Resolve<T>(model.Base);
        }

        private T GetComponent<T>(string name, IUnityContainer parentScope)
        {
            if (!_componentModels.TryGetValue(name, out var model))
                throw new ArgumentException($"The component {name} is not defined");

            // Components themselves gain yet another scope, to keep them isolated
            // from multiple components of the same type being used in one agent
            var componentScope = parentScope.CreateChildContainer();

            componentScope.RegisterInstance<IUnityContainer>(componentScope);
            componentScope.RegisterInstance<IConfig>(GetConfig(name, _parentConfig));

            return componentScope.Resolve<T>(model.Base);
        }

        /// <summary>
        /// Returns (or creates, if necessary), a scoped IConfig instance for the named component
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private IConfig GetConfig(string name, IConfig parent)
        {
            if (!_componentConfigs.ContainsKey(name))
            {
                var componentModel = _componentModels[name];
                var componentConfig = new DictionaryConfig(componentModel.Config, parent);

                _componentConfigs.Add(name, componentConfig);
            }

            return _componentConfigs[name];
        }
    }
}
