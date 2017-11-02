using System.Collections.Generic;
using System.Linq;
using Catch.Base;
using Catch.Graphics;

namespace Catch.Level
{
    /// <summary>
    /// A simple class to be reponsible for delegating Graphics Resource events
    /// from the <see cref="LevelController"/> to all instances of 
    /// <see cref="IGraphicsResourceContainer"/>
    /// </summary>
    public class GraphicsResourceManager : IGraphicsResource
    {
        private readonly List<IGraphicsResource> _providers;

        public GraphicsResourceManager(IEnumerable<IProvider> providers)
        {
            _providers = providers.OfType<IGraphicsResource>().ToList();
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            foreach (var provider in _providers)
                provider.CreateResources(args);
        }

        public void DestroyResources()
        {
            foreach (var provider in _providers)
                provider.DestroyResources();
        }
    }
}
