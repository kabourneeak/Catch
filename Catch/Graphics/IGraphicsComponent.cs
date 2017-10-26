namespace Catch.Graphics
{
    /// <summary>
    /// A component which can draw an <see cref="IDrawable"/>
    /// </summary>
    public interface IGraphicsComponent
    {
        /// <summary>
        /// Draw the drawable with the current draw arguments
        /// </summary>
        /// <param name="drawable">The object to draw</param>
        /// <param name="drawArgs">The DrawArgs to draw against, which includes the output device</param>
        void Draw(IDrawable drawable, DrawArgs drawArgs);
    }
}
