using System;
using System.Collections;
using System.Collections.Generic;
using Catch.Base;

namespace Catch.Models
{
    public class BasicIndicatorCollection : IIndicatorCollection
    {
        private readonly SortedSet<IIndicator> _indicators;

        public BasicIndicatorCollection()
        {
            _indicators = new SortedSet<IIndicator>(IndicatorComparer.GetComparer());
        }

        public IEnumerator<IIndicator> GetEnumerator()
        {
            return _indicators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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

        public void Draw(DrawArgs drawArgs)
        {
            foreach (var i in _indicators)
                i.Draw(drawArgs);
        }

        public void Add(IIndicator indicator)
        {
            _indicators.Add(indicator);
        }

        public void Remove(IIndicator indicator)
        {
            _indicators.Remove(indicator);
        }

        public int Count { get { return _indicators.Count; } }

        public IIndicator HasIndicator(string indicatorType)
        {
            throw new NotImplementedException();
        }

        private class IndicatorComparer : IComparer<IIndicator>
        {
            private static IndicatorComparer _instance;

            public static IndicatorComparer GetComparer()
            {
                return _instance ?? (_instance = new IndicatorComparer());
            }

            private IndicatorComparer()
            {
                
            }

            public int Compare(IIndicator x, IIndicator y)
            {
                return x.Layer.CompareTo(y.Layer);
            }
        }
    }
}
