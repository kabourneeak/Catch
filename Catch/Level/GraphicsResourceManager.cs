using System.Collections.Generic;
using System.Linq;
using Catch.Base;
using Catch.Graphics;
using Microsoft.Graphics.Canvas;

namespace Catch.Level
{
    /// <summary>
    /// A simple class to be reponsible for delegating Graphics Resource events
    /// from the <see cref="LevelController"/> to all instances of 
    /// <see cref="IGraphicsResourceContainer"/>
    /// </summary>
    public class GraphicsResourceManager : IGraphicsResourceContainer
    {
        private readonly List<IGraphicsResourceContainer> _providers;

        public GraphicsResourceManager(IEnumerable<IProvider> providers)
        {
            _providers = providers.OfType<IGraphicsResourceContainer>()
                .Where(p => !(p is GraphicsResourceManager))
                .ToList();
        }

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            foreach (var provider in _providers)
                provider.CreateResources(resourceCreator);
        }

        public void DestroyResources()
        {
            foreach (var provider in _providers)
                provider.DestroyResources();
        }
    }
}
