﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Catch.Graphics;

namespace Catch.Base
{
    /// <summary>
    /// Maintains a collection of Indicators, and which can be acted upon as 
    /// an indicator itself
    /// </summary>
    public class IndicatorCollection : IEnumerable<IIndicator>, IIndicator, IGraphicsResource
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

        #region IIndicator

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            foreach (var i in _indicators)
                if (i.Layer == drawArgs.Layer && i.LevelOfDetail.HasFlag(drawArgs.LevelOfDetail))
                    i.Draw(drawArgs, rotation);
        }

        /// <summary>
        /// Returns the highest DrawLayer in the collection
        /// </summary>
        public DrawLayer Layer => _indicators.LastOrDefault()?.Layer ?? DrawLayer.Background;

        public DrawLevelOfDetail LevelOfDetail => DrawLevelOfDetail.All;

        #endregion

        #region IGraphicsResource

        public void CreateResources(CreateResourcesArgs args)
        {
            foreach (var indicator in _indicators)
                if (indicator is IGraphicsResource gr)
                    gr.CreateResources(args);
        }

        public void DestroyResources()
        {
            foreach (var indicator in _indicators)
                if (indicator is IGraphicsResource gr)
                    gr.DestroyResources();
        }

        #endregion

        public void Add(IIndicator indicator)
        {
            _indicators.Add(indicator);

            /*
             * Sort on add; C# will use Insertion Sort for small collections (< 16 items), 
             * which is efficient when the elements are mostly sorted.
             */

            // TODO now that we draw by layer, do we need to bother sorting this?
            // stored in a dictionary keyed by layer might be better

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
