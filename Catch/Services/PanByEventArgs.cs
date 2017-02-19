using System.Numerics;

namespace Catch.Services
{
    public class PanByEventArgs : EventArgsBase
    {
        public Vector2 PanDelta { get; }

        public PanByEventArgs(Vector2 panDelta)
        {
            PanDelta = panDelta;
        }
    }
}
