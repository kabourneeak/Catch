using Windows.UI;

namespace Catch.Base
{
    /// <inheritdoc />
    public class BaseStatsModel : IBaseStats
    {
        public string DisplayName { get; set; }

        public string DisplayInfo { get; set; }

        public string DisplayStatus { get; set; }

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

        public BaseStatsModel()
        {

        }
    }
}
