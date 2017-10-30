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
            UnityUtils.RegisterAllAsSingletons(typeof(IGraphicsProvider), container);

            return container;
        }
    }
}
