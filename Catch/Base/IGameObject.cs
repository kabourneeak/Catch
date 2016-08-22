using System.Numerics;
using Catch.Graphics;

namespace Catch.Base
{
    public interface IGameObject : IGraphicsComponent
    {
        string DisplayName { get; }

        string DisplayInfo { get; }

        string DisplayStatus { get; }

        Vector2 Position { get; set; }

        float Rotation { get; set; }
    }
}
