using System;
using System.Collections;
using System.Collections.Generic;

namespace Catch.Base
{
    /// <summary>
    /// Maintains a collection of Indicators, and updates the <see cref="IIndicatorRegistry"/>
    /// with any changes
    /// </summary>
    public class IndicatorCollection : IEnumerable<IIndicator>
    {
        private readonly IIndicatorRegistry _registry;

        private readonly List<IIndicator> _indicators;

        public IndicatorCollection(IIndicatorRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));

            _indicators = new List<IIndicator>();
        }

        #region IEnumerable

        public IEnumerator<IIndicator> GetEnumerator() => _indicators.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        public void Add(IIndicator indicator)
        {
            _indicators.Add(indicator);
            _registry.Register(indicator);
        }

        public void AddRange(IEnumerable<IIndicator> collection)
        {
            foreach (var ind in collection)
                _indicators.Add(ind);
        }

        public void Remove(IIndicator indicator)
        {
            _indicators.Remove(indicator);
            _registry.Unregister(indicator);
        }

        public int Count => _indicators.Count;
    }
}
