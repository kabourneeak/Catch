using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Level
{
    public class GraphicsManager : IGraphicsManager
    {
        private readonly IConfig _config;
        private readonly Dictionary<Type, IGraphicsProvider> _providers;

        public GraphicsManager(IConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _providers = LoadGraphicsProviders();
        }

        public T Resolve<T>() where T : IGraphicsProvider
        {
            if (_providers.TryGetValue(typeof(T), out var provider))
            {
                return (T)provider;
            }
            else
            {
                throw new ArgumentException($"No instance of {typeof(T).FullName} was found");
            }
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            foreach (var provider in _providers.Values)
                provider.CreateResources(args);
        }

        public void DestroyResources()
        {
            foreach (var provider in _providers.Values)
                provider.DestroyResources();
        }

        private Dictionary<Type, IGraphicsProvider> LoadGraphicsProviders()
        {
            var agentFactories = new Dictionary<Type, IGraphicsProvider>();

            // Get the current assembly through the current class
            var currentAssembly = this.GetType().GetTypeInfo().Assembly;

            // Filter the defined classes according to the interfaces they implement
            var graphicsProviderClasses = currentAssembly
                .DefinedTypes
                .Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(IGraphicsProvider)));

            foreach (var clazz in graphicsProviderClasses)
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
                    else
                    {
                        throw new NotSupportedException($"Cannot supply ctor arg of type {ctorArg.ParameterType}");
                    }
                }

                // create the object
                var inst = (IGraphicsProvider)ctor.Invoke(ctorArgs.ToArray());

                // add to dictionary
                agentFactories.Add(inst.GetType(), inst);
            }

            return agentFactories;
        }
    }
}
