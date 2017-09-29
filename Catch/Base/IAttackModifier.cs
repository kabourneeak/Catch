namespace Catch.Base
{
    public interface IAttackModifier : IModifier
    {
        void OnAttack(IExtendedAgent agent, AttackEventArgs e);
    }
}