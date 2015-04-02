using Catch.Base;

namespace Catch.Models
{
    public class EmptyBrain : IBehaviourComponent
    {
        public void OnSpawn()
        {
            // do nothing
        }

        public bool Update(float ticks)
        {
            return true;
        }

        public void OnRemove()
        {
            // do nothing
        }

        public void OnAttacked(IAttack attack)
        {
            // do nothing
        }
    }
}
