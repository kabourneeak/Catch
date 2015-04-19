using Catch.Base;

namespace Catch.Models
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
            var bs = Agent.BaseSpecs;

            bs.MaxHealth = 100;
            bs.Health = 100;
            bs.Level = 1;

            NeedsApplyToBase = false;
        }

        public override bool ApplyToAttack(AttackModel outgoingAttack)
        {
            // This is a base modifier, so we override any existing value
            outgoingAttack.Damage = (int) (BaseDamage * Intensity * Agent.BaseSpecs.Level);

            return true;
        }
    }
}
