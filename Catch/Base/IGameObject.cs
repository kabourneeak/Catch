using System.Numerics;

namespace Catch.Base
{
    public interface IGameObject : IGraphicsComponent
    {
        // properties
        string DisplayName { get; }

        string DisplayInfo { get; }

        string DisplayStatus { get; }

        Vector2 Position { get; set; }

        DrawLayer Layer { get; }
    }
}
