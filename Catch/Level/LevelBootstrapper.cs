using System.IO;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using CatchLibrary.Serialization.Assets;
using Newtonsoft.Json;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Catch.Level
{
    public static class LevelBootstrapper
    {
        private static readonly string CfgAssets = ConfigUtils.GetConfigPath(nameof(LevelBootstrapper), nameof(CfgAssets));

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
                new InjectionFactory(c => new GraphicsManager(c.ResolveAll<IProvider>())));

            /*
             * Register Models
             */
            container.RegisterType<MapModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMap, MapModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<UiStateModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<SimulationStateModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISimulationState, SimulationStateModel>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<AssetModel>(LoadAssetModel(config));

            /*
             * Register controllers
             */
            container.RegisterType<UpdateController>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISimulationManager, SimulationManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<FieldController>(new ContainerControlledLifetimeManager());
            container.RegisterType<OverlayController>(new ContainerControlledLifetimeManager());
            container.RegisterType<StatusBar>(new ContainerControlledLifetimeManager());

            /*
             * Register Providers
             */
            UnityUtils.RegisterAllAsSingletons(typeof(IProvider), container);

            container.RegisterType<ILabelProvider>(new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => c.Resolve<LabelProvider>()));
            container.RegisterType<IAgentProvider>(new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => c.Resolve<AgentProvider>()));


            /*
             * Register Agent Components
             */
            UnityUtils.RegisterAllAsTransient(typeof(IGraphicsComponent), container);
            UnityUtils.RegisterAllAsTransient(typeof(IModifier), container);
            UnityUtils.RegisterAllAsTransient(typeof(IUpdatable), container);
            UnityUtils.RegisterAllAsTransient(typeof(IIndicator), container);
            UnityUtils.RegisterAllAsTransient(typeof(ISprite), container);

            return container;
        }

        private static AssetModel LoadAssetModel(IConfig config)
        {
            var filename = config.GetString(CfgAssets);
            var assetModel = JsonConvert.DeserializeObject<AssetModel>(File.ReadAllText(filename));

            return assetModel;
        }
    }
}
