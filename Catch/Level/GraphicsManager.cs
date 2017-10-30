using System.Collections.Generic;
using System.Linq;
using Catch.Graphics;

namespace Catch.Level
{
    /// <summary>
    /// A simple class to be reponsible for delegating Graphics Resource events
    /// from the <see cref="LevelController"/> to all instances of 
    /// <see cref="IGraphicsProvider"/>
    /// </summary>
    public class GraphicsManager : IGraphicsResource
    {
        private readonly List<IGraphicsProvider> _providers;

        public GraphicsManager(IEnumerable<IGraphicsProvider> providers)
        {
            _providers = providers.ToList();
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
