namespace Catch.Base
{
    /// <summary>
    /// Indicators represent components of a graphical object. For example, a Tower may be drawn
    /// as the sum of several Indicators; one for the base image, one for level indicataors, 
    /// another for health, and so on.
    /// 
    /// Indicators should expect to be drawn relative to their parent object. The parent object
    /// is responsible for setting draw transformations appropriately in DrawArgs.
    /// </summary>
    public interface IIndicator : IGraphicsComponent
    {
        // Properties
        DrawLayer Layer { get; }
    }
}