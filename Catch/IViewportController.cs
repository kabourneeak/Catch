using System.Numerics;
using Windows.Foundation;

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

        // TODO will need something here for mouse movement and selection which are not pan/zoom events; keyboard too
    }
}