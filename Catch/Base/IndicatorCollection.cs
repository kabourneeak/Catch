using System.Collections;
using System.Collections.Generic;
using Catch.Graphics;

namespace Catch.Base
{
    /// <summary>
    /// Maintains a collection of Indicators, and which can be acted upon as 
    /// an indicator itself
    /// </summary>
    public class IndicatorCollection : IEnumerable<IIndicator>
    {
        private static readonly IComparer<IIndicator> IndicatorComparer =
            Comparer<IIndicator>.Create((x, y) => x.Layer.CompareTo(y.Layer));

        private readonly List<IIndicator> _indicators;

        public IndicatorCollection()
        {
            _indicators = new List<IIndicator>();
        }

        #region IEnumerable

        public IEnumerator<IIndicator> GetEnumerator() => _indicators.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        public void Add(IIndicator indicator)
        {
            _indicators.Add(indicator);

            /*
             * Sort on add; C# will use Insertion Sort for small collections (< 16 items), 
             * which is efficient when the elements are mostly sorted.
             */

            _indicators.Sort(IndicatorComparer);
        }

        public void AddRange(IEnumerable<IIndicator> collection)
        {
            foreach (var ind in collection)
                _indicators.Add(ind);
        }

        public void Remove(IIndicator indicator)
        {
            _indicators.Remove(indicator);
        }

        public int Count => _indicators.Count;
    }
}
