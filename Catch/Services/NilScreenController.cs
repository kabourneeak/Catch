using System.Numerics;
using Catch.Graphics;

namespace Catch.Services
{
    /// <summary>
    /// An empty IScreenController that an IScreenManager can call
    /// when no other controller is available
    /// </summary>
    public class NilScreenController : IScreenController
    {
        #region IScreenController Implementation

        public void Initialize(Vector2 size)
        {
            // do nothing
        }

        public bool AllowPredecessorUpdate() => false;

        public bool AllowPredecessorDraw() => false;

        public bool AllowPredecessorInput() => false;

        public void Update(float deviceTicks)
        {
            // do nothing
        }

        #endregion

        #region IGraphicsResource Implementation

        public void CreateResources(CreateResourcesArgs args)
        {
            // do nothing
        }

        public void DestroyResources()
        {
            // do nothing
        }

        #endregion

        #region IDrawable Implementation

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
