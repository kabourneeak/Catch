using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Catch.Base
{
    public interface IGraphicsComponent
    {
        // Properties
        Vector2 Position { get; }

        DrawLayer Layer { get; }

        // Events
        void Update(float ticks);

        void CreateResources(CanvasDrawingSession drawingSession);

        void Draw(CanvasDrawingSession drawingSession);
    }

    public enum DrawLayer
    {
        Background, Base, Tower, Agent, Mob, Effect, Ui
    }

}