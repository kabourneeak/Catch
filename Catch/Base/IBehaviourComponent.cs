namespace Catch.Base
{
    public interface IBehaviourComponent : IUpdatable
    {
        void OnRemove();

        void OnHit(AttackModel incomingAttack);
    }
}