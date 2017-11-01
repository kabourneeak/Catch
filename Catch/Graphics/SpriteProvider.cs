using System;
using System.Collections.Generic;
using Catch.Services;
using CatchLibrary.Serialization.Assets;
using Unity;

namespace Catch.Graphics
{
    public class SpriteProvider : IProvider, IGraphicsProvider
    {
        private readonly IConfig _config;
        private readonly IUnityContainer _container;
        private Dictionary<string, ComponentModel> _models;
        private Dictionary<string, ISprite> _sprites;

        public SpriteProvider(IConfig config, AssetModel assetModel, IUnityContainer container)
        {
            _config = config;
            _container = container;
            _models = new Dictionary<string, ComponentModel>();
            _sprites = new Dictionary<string, ISprite>();

            foreach (var spriteModel in assetModel.Sprites)
            {
                _models.Add(spriteModel.Name, spriteModel);
            }
        }

        public ISprite GetSprite(string spriteName)
        {
            // TODO make sure that we are actually getting singletons

            if (_sprites.TryGetValue(spriteName, out var existingSprite))
            {
                return existingSprite;
            }
            else
            {
                var loadedSprite = LoadSprite(spriteName);

                // save instance for next time
                _sprites.Add(spriteName, loadedSprite);

                return loadedSprite;
            }
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            foreach (var sprite in _sprites.Values)
                sprite.CreateResources(args);
        }

        public void DestroyResources()
        {
            foreach (var sprite in _sprites.Values)
                sprite.DestroyResources();
        }

        private ISprite LoadSprite(string spriteName)
        {
            if (_models.TryGetValue(spriteName, out var spriteModel))
            {
                // create sprite-scoped container
                var spriteContainer = _container.CreateChildContainer();

                var spriteConfig = new DictionaryConfig(spriteModel.Config, _config);
                spriteContainer.RegisterInstance<IConfig>(spriteConfig);

                // create instance
                var sprite = spriteContainer.Resolve<ISprite>(spriteModel.Base);

                return sprite;
            }
            else
            {
                throw new ArgumentException($"No such sprite named {spriteName} is defined");
            }
        }
    }
}
