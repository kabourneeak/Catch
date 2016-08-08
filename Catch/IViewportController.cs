using System.Numerics;
using Windows.System;

namespace Catch
{
    /// <summary>
    /// Handles highlevel view port events such as mouse movement, pan and zoom, resize
    /// </summary>
    public interface IViewportController
    {
        void PanBy(Vector2 panDelta);

        void ZoomToPoint(Vector2 viewCoords, float zoomDelta);

        void Resize(Vector2 size);

        void Hover(Vector2 viewCoords, VirtualKeyModifiers keyModifiers);

        void Touch(Vector2 viewCoords, VirtualKeyModifiers keyModifiers);
        
        void KeyPress(VirtualKey key);
    }
}