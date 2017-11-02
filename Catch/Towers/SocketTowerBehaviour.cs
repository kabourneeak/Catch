using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    /// <summary>
    /// A basic tower that just exposes indicators and commands
    /// </summary>
    public class SocketTowerBehaviour : NilAgentBehaviour
    {
        private static readonly string CfgTextIndicatorName = ConfigUtils.GetConfigPath(nameof(SocketTowerBehaviour), nameof(CfgTextIndicatorName));

        public SocketTowerBehaviour(IConfig config, IExtendedAgent host, IIndicatorProvider indicatorProvider)
        {
            host.Position = host.Tile.Position;

            var labelIndicatorName = config.GetString(CfgTextIndicatorName);
            var labelIndicator = (TextIndicator)indicatorProvider.GetIndicator(labelIndicatorName);

            var labelText = string.Format("{0},{1}", host.Tile.Coords.Q, host.Tile.Coords.R);
            labelIndicator.SetLabelText(labelText);

            host.Indicators.Add(labelIndicator);
        }
    }
}
