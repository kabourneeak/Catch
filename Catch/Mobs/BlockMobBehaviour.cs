using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Mobs
{
    public class BlockMobBehaviour : PathMobBehaviour, IAgentStatsModifier
    {
        public const string AgentTypeName = "BlockMob";
        public static readonly string CfgBlockSize = ConfigUtils.GetConfigPath(AgentTypeName, nameof(CfgBlockSize));
        public static readonly string CfgVelocity = ConfigUtils.GetConfigPath(AgentTypeName, nameof(CfgVelocity));

        private readonly IConfig _config;

        public BlockMobBehaviour(IExtendedAgent host, IConfig config, BlockMobGraphicsProvider resources, IMapPath path) : base(host, path)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            if (resources == null) throw new ArgumentNullException(nameof(resources));
            if (path == null) throw new ArgumentNullException(nameof(path));

            host.Indicators.AddRange(resources.Indicators);
        }

        public void OnCalculateAgentStats(IExtendedAgent agent)
        {
            agent.ExtendedStats.MovementSpeed = _config.GetFloat(CfgVelocity);
        }
    }
}
