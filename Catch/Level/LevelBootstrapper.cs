using System;
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
        private static readonly string CfgModelsFolder = ConfigUtils.GetConfigPath(nameof(LevelBootstrapper), nameof(CfgModelsFolder));

        public static IUnityContainer CreateContainer(IConfig config)
        {
            var container = new UnityContainer();

            /*
             * Register services
             */

            container.RegisterInstance<IConfig>(config);

            // used during provision of agents, components to provide dependencies and scoped containers
            container.RegisterInstance<IUnityContainer>(container);

            container.RegisterType<GraphicsResourceManager>(
                new ContainerControlledLifetimeManager(), 
                new InjectionFactory(c => new GraphicsResourceManager(c.ResolveAll<IProvider>())));

            /*
             * Register Models
             */
            container.RegisterType<MapModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMap, MapModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<UiStateModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<SimulationStateModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISimulationState, SimulationStateModel>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<AssetModel>(LoadAssetModels(config));

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

        private static AssetModel LoadAssetModels(IConfig config)
        {
            var modelsFolderName = config.GetString(CfgModelsFolder);

            var appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var modelsFolder = appInstalledFolder.GetFolderAsync(modelsFolderName)
                    .GetAwaiter()
                    .GetResult();

            var modelFiles = modelsFolder.GetFilesAsync()
                .GetAwaiter()
                .GetResult();

            var overallAssetModel = new AssetModel();

            foreach (var modelFile in modelFiles)
            {
                try
                {
                    var assetModel = JsonConvert.DeserializeObject<AssetModel>(File.ReadAllText(modelFile.Path));

                    overallAssetModel.Agents.AddRange(assetModel.Agents);
                    overallAssetModel.Behaviours.AddRange(assetModel.Behaviours);
                    overallAssetModel.Colors.AddRange(assetModel.Colors);
                    overallAssetModel.Commands.AddRange(assetModel.Commands);
                    overallAssetModel.Indicators.AddRange(assetModel.Indicators);
                    overallAssetModel.Modifiers.AddRange(assetModel.Modifiers);
                    overallAssetModel.Sprites.AddRange(assetModel.Sprites);
                    overallAssetModel.Styles.AddRange(assetModel.Styles);
                }
                catch (IOException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new IOException($"Error processing model file {modelFile.Path}", e);
                }
            }

            return overallAssetModel;  
        }
    }
}
