using System;
using System.Linq;
using System.Reflection;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Catch.Level
{
    public static class LevelBootstrapper
    {
        public static IUnityContainer CreateContainer(IConfig config)
        {
            var container = new UnityContainer();

            /*
             * Register services
             */

            container.RegisterInstance<IConfig>(config);

            // used during provision of agents, components to provide dependencies and scoped containers
            container.RegisterInstance<IUnityContainer>(container);

            container.RegisterType<GraphicsManager>(
                new ContainerControlledLifetimeManager(), 
                new InjectionFactory(c => new GraphicsManager(c.ResolveAll<IGraphicsProvider>())));
            container.RegisterType<ILabelProvider, LabelProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAgentProvider, BuiltinAgentProvider>(new ContainerControlledLifetimeManager());

            /*
             * Register Models
             */
            container.RegisterType<MapModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMap, MapModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<UiStateModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<SimulationStateModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISimulationState, SimulationStateModel>(new ContainerControlledLifetimeManager());

            /*
             * Register controllers
             */
            container.RegisterType<UpdateController>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISimulationManager, SimulationManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<FieldController>(new ContainerControlledLifetimeManager());
            container.RegisterType<OverlayController>(new ContainerControlledLifetimeManager());
            container.RegisterType<StatusBar>(new ContainerControlledLifetimeManager());

            /*
             * Register Component Implementations
             */
            RegisterAllAsSingletons(typeof(IGraphicsProvider), container);

            return container;
        }

        private static void RegisterAllAsSingletons(Type ofType, IUnityContainer container)
        {
            // Get the current assembly through the current class
            var currentAssembly = typeof(LevelBootstrapper).GetTypeInfo().Assembly;

            // Filter the defined classes according to the interfaces they implement
            var implTypeInfos = currentAssembly
                .DefinedTypes
                .Where(type => type.ImplementedInterfaces.Any(inter => inter == ofType));

            foreach (var implTypeInfo in implTypeInfos)
            {
                var implType = implTypeInfo.AsType();

                // Unity does not share instances for multiple registrations when using 
                // named registrations, so we need to instantiate it now and register the 
                // instances instead.
                var inst = container.Resolve(implType);

                // register as the concrete type
                container.RegisterInstance(implType, inst);

                // register as the interface type (for resolve all)
                container.RegisterInstance(ofType, implTypeInfo.Name, inst);
            }
        }
    }
}
