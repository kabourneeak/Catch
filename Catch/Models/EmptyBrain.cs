using Catch.Base;

namespace Catch.Models
{
    public class EmptyBrain : IBehaviourComponent
    {
        public void OnSpawn()
        {
            // do nothing
        }

        public void Update(float ticks)
        {
            // do nothing
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
