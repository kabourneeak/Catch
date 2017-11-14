using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Level
{
    public class IndicatorRegistry : IIndicatorRegistry
    {
        private static readonly DrawLayer[] DrawLayers = EnumUtils.GetEnumAsList<DrawLayer>().ToArray();

        private readonly IVersionedCollection<IIndicator>[] _highLod;
        private readonly IVersionedCollection<IIndicator>[] _normalLod;
        private readonly IVersionedCollection<IIndicator>[] _lowLod;

        public IndicatorRegistry()
        {
            InitLodCollection(out _highLod);
            InitLodCollection(out _normalLod);
            InitLodCollection(out _lowLod);
        }

        public void Register(IIndicator indicator)
        {
            if (indicator.LevelOfDetail.HasFlag(DrawLevelOfDetail.High))
                RegisterInLodCollection(_highLod, indicator);
            if (indicator.LevelOfDetail.HasFlag(DrawLevelOfDetail.Normal))
                RegisterInLodCollection(_normalLod, indicator);
            if (indicator.LevelOfDetail.HasFlag(DrawLevelOfDetail.Low))
                RegisterInLodCollection(_lowLod, indicator);
        }

        public void Register(IEnumerable<IIndicator> indicators)
        {
            foreach (var indicator in indicators)
                Register(indicator);
        }

        public void Unregister(IIndicator indicator)
        {
            if (indicator.LevelOfDetail.HasFlag(DrawLevelOfDetail.High))
                UnregisterFromLodCollection(_highLod, indicator);
            if (indicator.LevelOfDetail.HasFlag(DrawLevelOfDetail.Normal))
                UnregisterFromLodCollection(_normalLod, indicator);
            if (indicator.LevelOfDetail.HasFlag(DrawLevelOfDetail.Low))
                UnregisterFromLodCollection(_lowLod, indicator);
        }

        public void Unregister(IEnumerable<IIndicator> indicators)
        {
            foreach (var indicator in indicators)
                Unregister(indicator);
        }

        public int GetVersion(DrawLevelOfDetail lod, DrawLayer layer)
        {
            switch (lod)
            {
                case DrawLevelOfDetail.Low:
                    return _lowLod[(int) layer].Version;
                case DrawLevelOfDetail.Normal:
                    return _normalLod[(int)layer].Version;
                case DrawLevelOfDetail.High:
                    return _highLod[(int)layer].Version;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lod), lod, null);
            }  
        }

        public IEnumerable<IIndicator> GetIndicators(DrawLevelOfDetail lod, DrawLayer layer)
        {
            switch (lod)
            {
                case DrawLevelOfDetail.Low:
                    return _lowLod[(int)layer];
                case DrawLevelOfDetail.Normal:
                    return _normalLod[(int)layer];
                case DrawLevelOfDetail.High:
                    return _highLod[(int)layer];
                default:
                    throw new ArgumentOutOfRangeException(nameof(lod), lod, null);
            }
        }

        private void InitLodCollection(out IVersionedCollection<IIndicator>[] lodCollection)
        {
            lodCollection = new IVersionedCollection<IIndicator>[DrawLayers.Length];

            for (int i = 0; i < lodCollection.Length; ++i)
            {
                lodCollection[i] = new VersionedCollection<IIndicator>(new HashSet<IIndicator>());
            }
        }

        private void RegisterInLodCollection(IVersionedCollection<IIndicator>[] lodCollection, IIndicator indicator)
        {
            lodCollection[(int) indicator.Layer].Add(indicator);
        }

        private void UnregisterFromLodCollection(IVersionedCollection<IIndicator>[] lodCollection, IIndicator indicator)
        {
            lodCollection[(int)indicator.Layer].Remove(indicator);
        }
    }
}
