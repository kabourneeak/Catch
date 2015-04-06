namespace Catch.Base
{
    public interface IBehaviourComponent
    {
        void OnSpawn();

        void Update(float ticks);

        void OnRemove();

        void OnAttacked(IAttack attack);
    }
}