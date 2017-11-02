using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Services;
using CatchLibrary.Serialization.Assets;
using Unity;

namespace Catch.Components
{
    public class ModifierProvider : IProvider
    {
        private readonly IUnityContainer _container;
        private readonly Dictionary<string, ComponentModel> _models;
        private readonly Dictionary<string, IConfig> _configs;

        public ModifierProvider(IConfig config, AssetModel assetModel, IUnityContainer container)
        {
            _container = container;
            _models = new Dictionary<string, ComponentModel>();
            _configs = new Dictionary<string, IConfig>();

            foreach (var model in assetModel.Modifiers)
            {
                _models.Add(model.Name, model);
                _configs.Add(model.Name, new DictionaryConfig(model.Config, config));
            }
        }

        public IModifier GetModifier(string modifierName, IExtendedAgent host)
        {
            if (_models.TryGetValue(modifierName, out var im))
            {
                var scopedContainer = _container.CreateChildContainer();

                scopedContainer.RegisterInstance<IAgent>(host);
                scopedContainer.RegisterInstance<IExtendedAgent>(host);
                scopedContainer.RegisterInstance<IConfig>(_configs[modifierName]);

                var modifier = scopedContainer.Resolve<IModifier>(im.Base);

                return modifier;
            }

            throw new ArgumentException($"The modifier {modifierName} is not defined");
        }
    }
}
