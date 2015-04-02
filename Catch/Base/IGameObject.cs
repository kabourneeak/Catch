using Microsoft.Graphics.Canvas;
using System.Numerics;

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

        void CreateResources(CanvasDrawingSession ds);

        void Draw(CanvasDrawingSession ds);
    }

    public enum DrawLayer
    {
        Background, Base, Tower, Agent, Mob, Effect, Ui
    }
}
