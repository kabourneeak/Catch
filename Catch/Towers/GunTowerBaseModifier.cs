using Catch.Base;

namespace Catch.Towers
{
    public class GunTowerBaseModifier : ICalculateAgentStatsModifier
    {
        public ModifierPriority Priority => ModifierPriority.Base;

        public GunTowerBaseModifier()
        {

        }

        public void OnCalculateAgentStats(IExtendedAgent agent)
        {
            agent.ExtendedStats.MaxHealth = 100;
            agent.ExtendedStats.Health = 100;
            agent.ExtendedStats.Level = 1;
        }
    }
}
