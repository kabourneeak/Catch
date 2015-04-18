namespace Catch.Base
{
    public interface IBehaviourComponent
    {
        void Update(float ticks);

        void OnRemove();

        void OnAttacked(IAttack attack);
    }
}