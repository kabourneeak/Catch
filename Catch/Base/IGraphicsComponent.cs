namespace Catch.Base
{
    /// <summary>
    /// Just the basic methods for drawing and animation
    /// </summary>
    public interface IGraphicsComponent
    {
        // Events
        void Update(float ticks);

        void CreateResources(CreateResourcesArgs createArgs);

        /// <summary>
        /// Has the IGraphicsComponent draw itself.
        /// 
        /// Regarding the rotation parameter, it is up to the IGraphicsComponent to decide if it will use 
        /// this value. Non-optional rotation can be enforced by pushing a rotation transformation to the
        /// DrawArgs before calling Draw.
        /// </summary>
        /// <param name="drawArgs">The DrawArgs to draw against, which includes the output device</param>
        /// <param name="rotation">A hint from the caller as to the orientation of what is being drawn.</param>
        void Draw(DrawArgs drawArgs, float rotation);
    }
}