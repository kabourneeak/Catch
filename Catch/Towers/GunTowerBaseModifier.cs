using Catch.Base;

namespace Catch.Towers
{
    public class GunTowerBaseModifier : Modifier
    {
        private const int BaseDamage = 10;

        public GunTowerBaseModifier(IAgent agent) : base(agent)
        {
            Priority = ModifierPriority.Base;
        }

        public override void ApplyToBase()
        {
            var stats = Agent.Stats;

            stats.MaxHealth = 100;
            stats.Health = 100;
            stats.Level = 1;

            NeedsApplyToBase = false;
        }

        public override bool ApplyToAttack(AttackModel outgoingAttack)
        {
            // This is a base modifier, so we override any existing value
            outgoingAttack.Damage = (int) (BaseDamage * Intensity * Agent.Stats.Level);

            return true;
        }
    }
}
