using System.Collections.Generic;

namespace Catch.Base
{
    public interface IIndicatorRegistry
    {
        void Register(IIndicator indicator);

        void Register(IEnumerable<IIndicator> indicators);

        void Unregister(IIndicator indicator);

        void Unregister(IEnumerable<IIndicator> indicators);

        int Version { get; }

        IEnumerable<IIndicator> Indicators { get; }
    }
}
