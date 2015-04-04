namespace Catch
{
    internal static class FloatExtensions
    {
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
