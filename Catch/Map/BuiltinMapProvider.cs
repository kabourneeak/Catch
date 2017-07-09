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

        public MapModel CreateMap(int rows, int columns)
        {
            var map = new MapModel(_config, rows, columns);

            return map;
        }
    }
}
