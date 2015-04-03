using System.Numerics;
using Windows.Foundation;

namespace Catch.Base
{
    public interface IGameObject
    {
        // properties
        string DisplayName { get; }

        string DisplayInfo { get; }

        string DisplayStatus { get; }

        Vector2 Position { get; }

        DrawLayer Layer { get; }

        // Events
        void Update(float ticks);

        void CreateResources(CreateResourcesArgs createArgs);

        void Draw(DrawArgs drawArgs);
    }

    public enum DrawLayer
    {
        Background, Base, Tower, Agent, Mob, Effect, Ui
    }
}
