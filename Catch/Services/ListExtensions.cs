using System.Collections.Generic;

namespace Catch.Services
{
    public static class ListExtensions
    {
        public static IEnumerable<T> ReverseIterator<T>(this List<T> list)
        {
            var count = list.Count;

            for (var i = count - 1; i >= 0; --i)
            {
                yield return list[i];
            }
        }
    }
}
