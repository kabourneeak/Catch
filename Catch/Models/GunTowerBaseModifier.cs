using Catch.Base;

namespace Catch.Models
{
    class GunTowerBaseModifier : Modifier
    {
        public GunTowerBaseModifier(IAgent agent) : base(agent)
        {
            Priority = ModifierPriority.Base;
        }

        public override void Apply()
        {
            var bs = Agent.BaseSpecs;

            bs.MaxHealth = 100;
            bs.Health = 100;
            bs.Level = 1;

            NeedsApply = false;
        }

        public override void Update(float ticks)
        {
            // do nothing
        }
    }
}
