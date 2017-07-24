namespace Catch.Base
{
    public class NilBehaviour : IBehaviourComponent
    {
        public float Update(IUpdateEventArgs e)
        {
            // indicate that we don't require any more updates
            return 0.0f;
        }
     
        public void OnRemove()
        {
            // do nothing
        }

        public void OnHit(AttackModel incomingAttack)
        {
            // do nothing
        }
    }
}
