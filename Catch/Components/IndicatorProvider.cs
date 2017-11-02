using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Services;
using CatchLibrary.Serialization.Assets;
using Unity;

namespace Catch.Components
{
    /// <summary>
    /// Creates transient instances of <see cref="IIndicator"/>
    /// </summary>
    public class IndicatorProvider : IProvider
    {
        private readonly IUnityContainer _container;
        private readonly Dictionary<string, ComponentModel> _indicatorModels;
        private readonly Dictionary<string, IConfig> _configs;

        public IndicatorProvider(IConfig config, AssetModel assetModel, IUnityContainer container)
        {
            _configs = new Dictionary<string, IConfig>();
            _container = container;

            _indicatorModels = new Dictionary<string, ComponentModel>();

            foreach (var im in assetModel.Indicators)
            {
                _indicatorModels.Add(im.Name, im);
                _configs.Add(im.Name, new DictionaryConfig(im.Config, config));
            }
        }

        public IIndicator GetIndicator(string indicatorName)
        {
            if (_indicatorModels.TryGetValue(indicatorName, out var im))
            {
                var scopedContainer = _container.CreateChildContainer();

                scopedContainer.RegisterInstance<IConfig>(_configs[indicatorName]);

                var indicator = scopedContainer.Resolve<IIndicator>(im.Base);

                return indicator;
            }

            throw new ArgumentException($"The indicator {indicatorName} is not defined");
        }

        public IIndicator GetIndicator(string indicatorName, IExtendedAgent host)
        {
            if (_indicatorModels.TryGetValue(indicatorName, out var im))
            {
                var scopedContainer = _container.CreateChildContainer();

                scopedContainer.RegisterInstance<IAgent>(host);
                scopedContainer.RegisterInstance<IExtendedAgent>(host);
                scopedContainer.RegisterInstance<IConfig>(_configs[indicatorName]);

                var indicator = scopedContainer.Resolve<IIndicator>(im.Base);

                return indicator;
            }

            throw new ArgumentException($"The indicator {indicatorName} is not defined");
        }

        public IIndicator GetIndicator(string indicatorName, IMapTile mapTile)
        {
            if (_indicatorModels.TryGetValue(indicatorName, out var im))
            {
                var scopedContainer = _container.CreateChildContainer();

                scopedContainer.RegisterInstance<IMapTile>(mapTile);
                scopedContainer.RegisterInstance<IConfig>(_configs[indicatorName]);

                var indicator = scopedContainer.Resolve<IIndicator>(im.Base);

                return indicator;
            }

            throw new ArgumentException($"The indicator {indicatorName} is not defined");
        }
    }
}
