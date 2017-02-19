using System.Numerics;

namespace Catch.Services
{
    /// <summary>
    /// Handles highlevel view port events such as mouse movement, pan and zoom, resize
    /// </summary>
    public interface IViewportController
    {
        void PanBy(PanByEventArgs eventArgs);

        void ZoomToPoint(ZoomToPointEventArgs eventArgs);

        void Resize(Vector2 size);

        void Hover(HoverEventArgs eventArgs);

        void Touch(TouchEventArgs eventArgs);
        
        void KeyPress(KeyPressEventArgs eventArgs);
    }
}