using System.Numerics;

namespace Catch.Services
{
    public class ZoomToPointEventArgs : EventArgsBase
    {
        public Vector2 ViewCoords { get; }
        public float ZoomDelta { get; }

        public ZoomToPointEventArgs(Vector2 viewCoords, float zoomDelta)
        {
            ViewCoords = viewCoords;
            ZoomDelta = zoomDelta;
        }
    }
}
