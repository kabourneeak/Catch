using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Services;
using CatchLibrary.Serialization.Assets;
using Unity;

namespace Catch.Components
{
    public class BehaviourProvider : IProvider
    {
        private readonly IUnityContainer _container;

        private readonly Dictionary<string, ComponentModel> _models;
        private readonly Dictionary<string, IConfig> _configs;

        public BehaviourProvider(IConfig config, AssetModel assetModel, IUnityContainer container)
        {
            _container = container;
            _models = new Dictionary<string, ComponentModel>();
            _configs = new Dictionary<string, IConfig>();

            foreach (var model in assetModel.Behaviours)
            {
                _models.Add(model.Name, model);
                _configs.Add(model.Name, new DictionaryConfig(model.Config, config));
            }
        }

        public IUpdatable GetBehaviour(string behaviourName, IExtendedAgent host)
        {
            if (_models.TryGetValue(behaviourName, out var model))
            {
                var scopedContainer = _container.CreateChildContainer();

                scopedContainer.RegisterInstance<IAgent>(host);
                scopedContainer.RegisterInstance<IExtendedAgent>(host);
                scopedContainer.RegisterInstance<IConfig>(_configs[behaviourName]);

                var behaviour = scopedContainer.Resolve<IUpdatable>(model.Base);

                return behaviour;
            }

            throw new ArgumentException($"The behaviour {behaviourName} is not defined");
        }
    }
}
