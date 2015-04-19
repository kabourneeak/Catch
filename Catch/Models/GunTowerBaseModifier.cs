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

        public override void Update(float ticks)
        {
            // do nothing
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
            base.ApplyToAttack(outgoingAttack);

            // override any value with base damage
            outgoingAttack.Damage = Agent.BaseSpecs.Level * BaseDamage;

            return true;
        }
    }
}
