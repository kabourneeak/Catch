namespace Catch.Base
{
    public interface IBehaviourComponent
    {
        void Update(float ticks);

        void OnRemove();

        void OnHit(AttackModel incomingAttack);
    }
}