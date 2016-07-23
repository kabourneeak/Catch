using System;
using System.Collections;
using System.Collections.Generic;
using Catch.Graphics;

namespace Catch.Base
{
    public class IndicatorCollection : IEnumerable<IIndicator>, IGraphicsComponent
    {
        private readonly SortedSet<IIndicator> _indicators;

        public IndicatorCollection()
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

        public void Add(IIndicator indicator)
        {
            _indicators.Add(indicator);
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
