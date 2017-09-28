using Windows.UI;

namespace Catch.Base
{
    /// <summary>
    /// Describe the basic properties that agent which exist in the simulation exhibit
    /// 
    /// Rates are expressed in Update units.  A rate of 1.0 means one action per 1.0 Update tick.
    /// Intensity is a multiplier, where 1.0 is the base value
    /// </summary>
    public interface IBaseStats
    {
        int Health { get; }

        int MaxHealth { get; }

        float ColorResistence { get; }

        int Level { get; }

        Color Color { get; }

        int Team { get; }

        float MovementSpeed { get; }

        float AttackRate { get; }

        float AttackCost { get; }

        float AttackIntensity { get; }

        float AttackProcChance { get; }

        float ResourceValue { get; }

        float ExpValue { get; }

        float ResourceProductionRate { get; }
    }
}