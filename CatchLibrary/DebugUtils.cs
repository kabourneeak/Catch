using System.Diagnostics;

namespace CatchLibrary
{
    public static class DebugUtils
    {
        [Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            if (!condition)
                Debugger.Break();
        }
    }
}