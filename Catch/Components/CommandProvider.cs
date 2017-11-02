using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Services;
using CatchLibrary.Serialization.Assets;
using Unity;

namespace Catch.Components
{
    public class CommandProvider : IProvider
    {
        private readonly IUnityContainer _container;

        private readonly Dictionary<string, ComponentModel> _models;
        private readonly Dictionary<string, IConfig> _configs;

        public CommandProvider(IConfig config, AssetModel assetModel, IUnityContainer container)
        {
            _container = container;
            _models = new Dictionary<string, ComponentModel>();
            _configs = new Dictionary<string, IConfig>();

            foreach (var model in assetModel.Commands)
            {
                _models.Add(model.Name, model);
                _configs.Add(model.Name, new DictionaryConfig(model.Config, config));
            }
        }

        public IAgentCommand GetCommand(string commandName, IExtendedAgent host)
        {
            if (_models.TryGetValue(commandName, out var model))
            {
                var scopedContainer = _container.CreateChildContainer();

                scopedContainer.RegisterInstance<IAgent>(host);
                scopedContainer.RegisterInstance<IExtendedAgent>(host);
                scopedContainer.RegisterInstance<IConfig>(_configs[commandName]);

                var command = scopedContainer.Resolve<IAgentCommand>(model.Base);

                return command;
            }

            throw new ArgumentException($"The command {commandName} is not defined");
        }
    }
}
