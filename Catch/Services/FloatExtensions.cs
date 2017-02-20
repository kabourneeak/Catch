namespace Catch.Services
{
    internal static class FloatExtensions
    {
        /// <summary>
        /// Assumes that min \leq f \leq max, and that delta \le max - min
        /// </summary>
        internal static float Wrap(this float f, float delta, float min, float max)
        {
            f += delta;

            if (f > max)
                return (f - max) + min;
            if (f < min)
                return max - (min - f);
            return f;
        }
    }
}
