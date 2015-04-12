using System.Diagnostics;

namespace Catch
{
    public class DebugUtils
    {
        [Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            if (!condition)
                Debugger.Break();
        }
    }
}