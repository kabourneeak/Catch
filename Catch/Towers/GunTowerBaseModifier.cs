using Catch.Base;

namespace Catch.Towers
{
    public class GunTowerBaseModifier : IStatModifier<StatModel>
    {
        public GunTowerBaseModifier()
        {
            Intensity = 1.0f;
            Priority = ModifierPriority.Base;
        }

        public float Intensity { get; }

        public ModifierPriority Priority { get; }

        public void Apply(StatModel statModel)
        {
            statModel.MaxHealth = 100;
            statModel.Health = 100;
            statModel.Level = 1;
        }
    }
}
