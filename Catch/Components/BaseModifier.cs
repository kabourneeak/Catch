using Catch.Base;
using Catch.Services;

namespace Catch.Components
{
    /// <summary>
    /// Sets the Stats of an agent according to fixed, configured values.
    /// </summary>
    public class BaseModifier : IAgentStatsModifier
    {
        private static readonly string CfgMaxHealth = ConfigUtils.GetConfigPath(nameof(BaseModifier), nameof(CfgMaxHealth));
        private static readonly string CfgMovementSpeed = ConfigUtils.GetConfigPath(nameof(BaseModifier), nameof(CfgMovementSpeed));
        private static readonly string CfgLevel = ConfigUtils.GetConfigPath(nameof(BaseModifier), nameof(CfgLevel));
        private static readonly string CfgDisplayName = ConfigUtils.GetConfigPath(nameof(BaseModifier), nameof(CfgDisplayName));

        private readonly bool _hasMaxHealth;
        private readonly int _statMaxHealth;

        private readonly bool _hasMovementSpeed;
        private readonly float _statMovementSpeed;

        private readonly bool _hasLevel;
        private readonly int _statLevel;

        private readonly bool _hasDisplayName;
        private readonly string _statDisplayName;

        public BaseModifier(IConfig config)
        {
            _hasMaxHealth = config.HasKey(CfgMaxHealth);
            if (_hasMaxHealth) _statMaxHealth = config.GetInt(CfgMaxHealth);

            _hasMovementSpeed = config.HasKey(CfgMovementSpeed);
            if (_hasMovementSpeed) _statMovementSpeed = config.GetFloat(CfgMovementSpeed);

            _hasLevel = config.HasKey(CfgLevel);
            if (_hasLevel) _statLevel = config.GetInt(CfgLevel);

            _hasDisplayName = config.HasKey(CfgDisplayName);
            if (_hasDisplayName) _statDisplayName = config.GetString(CfgDisplayName);
        }

        public ModifierPriority Priority => ModifierPriority.Base;

        public void OnCalculateAgentStats(IExtendedAgent agent)
        {
            var stats = agent.ExtendedStats;

            if (_hasMaxHealth)
                stats.MaxHealth = _statMaxHealth;

            if (_hasMovementSpeed)
                stats.MovementSpeed = _statMovementSpeed;

            if (_hasLevel)
                stats.Level = _statLevel;

            if (_hasDisplayName)
                stats.DisplayName = _statDisplayName;
        }
    }
}
