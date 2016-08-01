using System.Diagnostics;

namespace CatchLibrary
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