namespace Catch.Base
{
    public interface IGameObject
    {
        // events
        void Update(float ticks);

        // components
        IGraphicsComponent Graphics { get; }

        // properties
        string DisplayName { get; }

        string DisplayInfo { get; }

        string DisplayStatus { get; }
    }
}
