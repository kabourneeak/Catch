using Catch.Services;

namespace Catch.Map
{
    public class BuiltinMapProvider : IMapProvider
    {
        private readonly IConfig _config;

        public BuiltinMapProvider(IConfig config)
        {
            _config = config;
        }

        public Map CreateMap(int rows, int columns)
        {
            var map = new Map(_config, rows, columns);

            return map;
        }
    }
}
