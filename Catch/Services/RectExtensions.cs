using System.Numerics;
using Windows.Foundation;

namespace Catch.Services
{
    public static class RectExtensions
    {
        /// <returns>the center point of the Rect</returns>
        public static Point Center(this Rect r)
        {
            return new Point((r.Left + r.Right) / 2, (r.Bottom + r.Top) / 2);
        }

        public static bool Contains(this Rect r, Vector2 p)
        {
            return r.Left <= p.X && r.Right >= p.X && r.Bottom <= p.Y && r.Top >= p.Y;
        }
    }
}
