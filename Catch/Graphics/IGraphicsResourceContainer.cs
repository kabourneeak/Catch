namespace Catch.Graphics
{
    /// <summary>
    /// Produces and maintains <see cref="IGraphicsResource"/> objects
    /// 
    /// Classes implementing this interface will be created as singletons during runtime and receive
    /// events for the <see cref="IGraphicsResource"/> objects they represent
    /// </summary>
    public interface IGraphicsResourceContainer : IGraphicsResource
    {
    }
}
