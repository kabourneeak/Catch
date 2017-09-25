using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Base
{
    /// <summary>
    /// An implementation of IAgentProvider that discovers agents throught reflection
    /// </summary>
    public class BuiltinAgentProvider : IAgentProvider
    {
        private readonly IConfig _config;
        private readonly ILabelProvider _labelProvider;
        private readonly Dictionary<string, IAgentFactory> _agentFactories;

        public BuiltinAgentProvider(IConfig config, ILabelProvider labelProvider)
        {
            _config = config;
            _labelProvider = labelProvider;

            // find IAgentFactories
            _agentFactories = LoadAgentFactories();
        }

        public IAgent CreateAgent(string name, CreateAgentArgs args)
        {
            var found = _agentFactories.TryGetValue(name, out IAgentFactory factory);

            if (!found)
                throw new ArgumentException($"I don't know how to construct an agent with name {name}");

            var agent = factory.CreateAgent(args);

            return agent;
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            foreach (var factory in _agentFactories.Values)
            {
                factory.CreateResources(args);
            }
        }

        public void DestroyResources()
        {
            foreach (var factory in _agentFactories.Values)
            {
                factory.DestroyResources();
            }
        }

        private Dictionary<string, IAgentFactory> LoadAgentFactories()
        {
            var agentFactories = new Dictionary<string, IAgentFactory>();

            // Get the current assembly through the current class
            var currentAssembly = this.GetType().GetTypeInfo().Assembly;

            // Filter the defined classes according to the interfaces they implement
            var agentFactoryClasses = currentAssembly
                .DefinedTypes
                .Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(IAgentFactory)));                

            foreach (var clazz in agentFactoryClasses)
            {
                var ctorArgs = new List<object>();
                var ctor = clazz.DeclaredConstructors.First();

                // look at each constructor argument and inject what is requested
                foreach (var ctorArg in ctor.GetParameters())
                {
                    if (ctorArg.ParameterType == typeof(IConfig))
                    {
                        ctorArgs.Add(_config);
                    }
                    else if (ctorArg.ParameterType == typeof(ILabelProvider))
                    {
                        ctorArgs.Add(_labelProvider);
                    }
                    else
                    {
                        throw new NotSupportedException($"Cannot supply ctor arg of type {ctorArg.ParameterType}");
                    }
                }

                // create the object
                var inst = (IAgentFactory) ctor.Invoke(ctorArgs.ToArray());

                // add to dictionary
                agentFactories.Add(inst.AgentType, inst);
            }

            return agentFactories;
        }
    }
}
