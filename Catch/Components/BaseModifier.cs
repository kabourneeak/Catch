using Catch.Base;
using Catch.Services;

namespace Catch.Components
{
    public class BaseModifier : IAgentStatsModifier
    {
        private static readonly string CfgMaxHealth = ConfigUtils.GetConfigPath(nameof(BaseModifier), nameof(CfgMaxHealth));
        private static readonly string CfgMovementSpeed = ConfigUtils.GetConfigPath(nameof(BaseModifier), nameof(CfgMovementSpeed));

        private readonly bool _hasMaxHealth;
        private readonly int _statMaxHealth;

        private readonly bool _hasMovementSpeed;
        private readonly float _statMovementSpeed;

        public BaseModifier(IConfig config)
        {
            _hasMaxHealth = config.HasKey(CfgMaxHealth);
            if (_hasMaxHealth) _statMaxHealth = config.GetInt(CfgMaxHealth);

            _hasMovementSpeed = config.HasKey(CfgMovementSpeed);
            if (_hasMovementSpeed) _statMovementSpeed = config.GetFloat(CfgMovementSpeed);
        }

        public ModifierPriority Priority => ModifierPriority.Base;

        public void OnCalculateAgentStats(IExtendedAgent agent)
        {
            var stats = agent.ExtendedStats;

            if (_hasMaxHealth)
                stats.MaxHealth = _statMaxHealth;

            if (_hasMovementSpeed)
                stats.MovementSpeed = _statMovementSpeed;
        }
    }
}
