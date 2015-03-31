namespace Catch.Base
{
    public interface IBehaviourComponent
    {
        void OnSpawn();

        bool Update(float ticks);

        void OnRemove();

        void OnAttacked(IAttack attack);
    }
}