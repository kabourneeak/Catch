using System.Collections.Generic;
using Catch.Base;
using Catch.Services;

namespace Catch.Level
{
    public class IndicatorRegistry : IIndicatorRegistry
    {
        private readonly IVersionedCollection<IIndicator> _indicators;

        public IndicatorRegistry()
        {
            _indicators = new VersionedCollection<IIndicator>(new HashSet<IIndicator>());
        }

        public void Register(IIndicator indicator)
        {
            _indicators.Add(indicator);
        }

        public void Register(IEnumerable<IIndicator> indicators)
        {
            foreach (var indicator in indicators)
                Register(indicator);
        }

        public void Unregister(IIndicator indicator)
        {
            _indicators.Remove(indicator);
        }

        public void Unregister(IEnumerable<IIndicator> indicators)
        {
            foreach (var indicator in indicators)
                Unregister(indicator);
        }

        public int Version => _indicators.Version;

        public IEnumerable<IIndicator> Indicators => _indicators;
    }
}
