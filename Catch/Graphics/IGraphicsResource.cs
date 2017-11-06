using Microsoft.Graphics.Canvas;

namespace Catch.Graphics
{
    /// <summary>
    /// An object which maintains graphical resources
    /// </summary>
    public interface IGraphicsResource
    {
        /// <summary>
        /// Indicates whether this resource has been created on the graphics hardware. This
        /// may become false over the course of the program. Consumers must check this flag
        /// and call <see cref="CreateResources"/> if it is false before drawing with this
        /// resource.
        /// </summary>
        bool IsCreated { get; }

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