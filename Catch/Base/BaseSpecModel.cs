namespace Catch.Base
{
    public class BaseSpecModel
    {
        public BaseSpecModel()
        {
            MaxHealth = 1;
            Health = 1;
            Level = 1;
        }

        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public int Level { get; set; }

        public float MovementSpeed { get; set; }
    }
}
