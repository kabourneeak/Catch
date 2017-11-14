using System.Collections.Generic;
using Catch.Graphics;

namespace Catch.Base
{
    public interface IIndicatorRegistry
    {
        void Register(IIndicator indicator);

        void Register(IEnumerable<IIndicator> indicators);

        void Unregister(IIndicator indicator);

        void Unregister(IEnumerable<IIndicator> indicators);

        int GetVersion(DrawLevelOfDetail lod, DrawLayer layer);

        IEnumerable<IIndicator> GetIndicators(DrawLevelOfDetail lod, DrawLayer layer);
    }
}
