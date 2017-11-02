using System;
using Catch.Base;
using Catch.Components;
using Catch.Services;

namespace Catch.Mobs
{
    public class BlockMobBehaviour : PathMobBehaviour, IAgentStatsModifier
    {
        public const string AgentTypeName = "BlockMob";
        public static readonly string CfgBlockSize = ConfigUtils.GetConfigPath(AgentTypeName, nameof(CfgBlockSize));
        public static readonly string CfgVelocity = ConfigUtils.GetConfigPath(AgentTypeName, nameof(CfgVelocity));

        private readonly IConfig _config;

        public BlockMobBehaviour(IExtendedAgent host, IConfig config, IndicatorProvider indicatorProvider, IMapPath path) : base(host, path)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            if (indicatorProvider == null) throw new ArgumentNullException(nameof(indicatorProvider));
            if (path == null) throw new ArgumentNullException(nameof(path));

            host.Indicators.Add(indicatorProvider.GetIndicator("BlockMobBaseIndicator"));
        }

        public void OnCalculateAgentStats(IExtendedAgent agent)
        {
            // TODO this should come from a base modifier somewhere

            agent.ExtendedStats.MovementSpeed = 0.005f;
        }
    }
}
