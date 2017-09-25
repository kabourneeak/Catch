using Catch.Services;

namespace Catch.Base
{
    /// <summary>
    /// Well known configuration values
    /// </summary>
    public static class CoreConfig
    {
        private const string CorePrefix = "Core";

        public static readonly string TileRadius = ConfigUtils.GetConfigPath(CorePrefix, nameof(TileRadius));
        public static readonly string TileRadiusInset = ConfigUtils.GetConfigPath(CorePrefix, nameof(TileRadiusInset));
        public static readonly string PlayerTeam = ConfigUtils.GetConfigPath(CorePrefix, nameof(PlayerTeam));
    }
}
