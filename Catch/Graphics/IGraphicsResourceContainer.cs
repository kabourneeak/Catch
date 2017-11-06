using Microsoft.Graphics.Canvas;

namespace Catch.Graphics
{
    /// <summary>
    /// Produces and maintains <see cref="IGraphicsResource"/> objects
    /// 
    /// Classes implementing this interface will be created as singletons during runtime and receive
    /// events for the <see cref="IGraphicsResource"/> objects they represent
    /// </summary>
    public interface IGraphicsResourceContainer
    {
        /// <summary>
        /// When called, an IGraphicsResource should create and cache any resources, e.g., 
        /// with Win2D CanvasCachedGeometry
        /// </summary>
        void CreateResources(ICanvasResourceCreator resourceCreator);

        /// <summary>
        /// When called, an IGraphicsResource should free any cached resources. The object may be called
        /// upon later to CreateResources.
        /// </summary>
        void DestroyResources();
    }
}
