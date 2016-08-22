namespace Catch.Graphics
{
    /// <summary>
    /// An object which can draw itself in Win2D
    /// </summary>
    public interface IGraphics
    {
        #region Events

        /// <summary>
        /// When called, an IGraphics should create and cache any resources, e.g., 
        /// with Win2D CanvasCachedGeometry
        /// </summary>
        void CreateResources(CreateResourcesArgs createArgs);

        /// <summary>
        /// When called, an IGraphics should free any cached resources. The object may be called
        /// upon later to CreateResources.
        /// </summary>
        void DestroyResources();

        /// <summary>
        /// Has the IGraphics draw itself.
        /// 
        /// Regarding the rotation parameter, it is up to the IGraphics to decide if it will use 
        /// this value. Non-optional rotation can be enforced by pushing a rotation transformation to the
        /// DrawArgs before calling Draw.
        /// </summary>
        /// <param name="drawArgs">The DrawArgs to draw against, which includes the output device</param>
        /// <param name="rotation">A hint from the caller as to the orientation of what is being drawn.</param>
        void Draw(DrawArgs drawArgs, float rotation);

        #endregion
    }
}