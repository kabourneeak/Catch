namespace Catch.Graphics
{
    /// <summary>
    /// An object which maintains graphical resources
    /// </summary>
    public interface IGraphicsResource
    {
        /// <summary>
        /// When called, an IGraphicsResource should create and cache any resources, e.g., 
        /// with Win2D CanvasCachedGeometry
        /// </summary>
        void CreateResources(CreateResourcesArgs args);

        /// <summary>
        /// When called, an IGraphicsResource should free any cached resources. The object may be called
        /// upon later to CreateResources.
        /// </summary>
        void DestroyResources();
    }
}