namespace Catch.Base
{
    public class AttackSpecModel
    {
        public AttackSpecModel()
        {
            Reset();
        }

        public float FiringInterval { get; set; }

        public void Reset()
        {
            FiringInterval = 0.0f;
        }
    }
}