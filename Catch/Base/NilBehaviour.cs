namespace Catch.Base
{
    public class NilBehaviour : IBehaviourComponent
    {
        public float Update(IUpdateEventArgs args)
        {
            // indicate that we don't require any more updates
            return 0.0f;
        }
     
        public void OnRemove()
        {
            // do nothing
        }

        public void OnChange(AttackModel attack)
        {
            // do nothing
        }
    }
}
