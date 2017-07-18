using Windows.UI;

namespace Catch.Base
{
    /// <summary>
    /// Describe the basic properties that agent which exist in the simulation exhibit
    /// 
    /// Rates are expressed in Update units.  A rate of 1.0 means one action per 1.0 Update tick.
    /// Intensity is a multiplier, where 1.0 is the base value
    /// </summary>
    public class StatModel
    {
        public StatModel()
        {
            Reset();
        }

        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public float ColorResistence { get; set; }

        public int Level { get; set; }

        public Color Color { get; set; }

        public int Team { get; set; }

        public float MovementSpeed { get; set; }

        public float AttackRate { get; set; }

        public float AttackCost { get; set; }

        public float AttackIntensity { get; set; }

        public float AttackProcChance { get; set; }

        public float ResourceValue { get; set; }

        public float ExpValue { get; set; }

        public float ResourceProductionRate { get; set; }

        public void Reset()
        {
            MaxHealth = 1;
            Health = 1;
            Level = 1;
            MovementSpeed = 0.0f;
            AttackRate = 0.0f;
        }
    }
}
