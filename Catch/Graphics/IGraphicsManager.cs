namespace Catch.Graphics
{
    /// <summary>
    /// Provides instances of <see cref="IGraphicsProvider"/> objects, and delegates
    /// calls necessary for <see cref="IGraphicsResource"/>
    /// </summary>
    public interface IGraphicsManager : IGraphicsResource
    {
        T Resolve<T>() where T : IGraphicsProvider;
    }
}
