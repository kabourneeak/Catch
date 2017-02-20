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
    }
}
