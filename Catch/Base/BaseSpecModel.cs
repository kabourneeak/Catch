namespace Catch.Base
{
    public class BaseSpecModel
    {
        public BaseSpecModel()
        {
            Reset();
        }

        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public int Level { get; set; }

        public float MovementSpeed { get; set; }
        public float FiringInterval { get; set; }

        public void Reset()
        {
            MaxHealth = 1;
            Health = 1;
            Level = 1;
            MovementSpeed = 0.0f;
            FiringInterval = 0.0f;
        }
    }
}
