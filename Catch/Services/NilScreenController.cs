using System.Numerics;
using Catch.Graphics;

namespace Catch.Services
{
    public class NilScreenController : IScreenController
    {
        #region IScreenController Implementation

        public void Initialize(Vector2 size, GameStateArgs args)
        {
            // do nothing
        }

        public bool AllowParentUpdate() => false;

        public bool AllowParentDraw() => false;

        public bool AllowParentInput() => false;

        #endregion

        #region IGraphicsComponent Implementation

        public void Update(float ticks)
        {
            // do nothing
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            // do nothing
        }

        public void DestroyResources()
        {
            // do nothing
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            // do nothing
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(PanByEventArgs eventArgs)
        {
            // do nothing
        }

        public void ZoomToPoint(ZoomToPointEventArgs eventArgs)
        {
            // do nothing
        }

        public void Resize(Vector2 size)
        {
            // do nothing
        }

        public void Hover(HoverEventArgs eventArgs)
        {
            // do nothing
        }

        public void Touch(TouchEventArgs eventArgs)
        {
            // do nothing
        }

        public void KeyPress(KeyPressEventArgs eventArgs)
        {
            // do nothing
        }

        #endregion
    }
}
