using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Catch.Graphics;

namespace Catch.Base
{
    /// <summary>
    /// Maintains a collcetion of Indicators, which can be acted upon as 
    /// an indicator itself
    /// </summary>
    public class IndicatorCollection : IEnumerable<IIndicator>, IIndicator
    {
        private readonly List<IIndicator> _indicators;
        private readonly IndicatorComparer _comparer;

        public IndicatorCollection()
        {
            _indicators = new List<IIndicator>();
            _comparer = new IndicatorComparer();
        }

        #region IEnumerable

        public IEnumerator<IIndicator> GetEnumerator()
        {
            return _indicators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IIndicator

        public void Update(float ticks)
        {
            foreach (var i in _indicators)
                i.Update(ticks);
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            foreach (var i in _indicators)
                i.CreateResources(createArgs);
        }

        public void DestroyResources()
        {
            foreach (var i in _indicators)
                i.DestroyResources();
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            foreach (var i in _indicators)
                i.Draw(drawArgs, rotation);
        }

        /// <summary>
        /// Returns the highest DrawLayer in the collection
        /// </summary>
        public DrawLayer Layer => _indicators.Last()?.Layer ?? DrawLayer.Background;

        #endregion

        public void Add(IIndicator indicator)
        {
            _indicators.Add(indicator);

            /*
             * Sort on add; C# will use Insertion Sort for small collections (< 16 items), 
             * which is efficient when the elements are mostly sorted.
             */
            _indicators.Sort(_comparer);
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

        public IIndicator HasIndicator(string indicatorType)
        {
            throw new NotImplementedException();
        }

        private class IndicatorComparer : IComparer<IIndicator>
        {
            public int Compare(IIndicator x, IIndicator y)
            {
                return x.Layer.CompareTo(y.Layer);
            }
        }
    }
}
