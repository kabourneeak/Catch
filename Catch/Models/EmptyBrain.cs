using Catch.Base;

namespace Catch.Models
{
    public class EmptyBrain : IBehaviourComponent
    {
        public void Update(float ticks)
        {
            // do nothing
        }

        public void OnRemove()
        {
            // do nothing
        }

        public void OnAttacked(AttackModel attack)
        {
            // do nothing
        }
    }
}
