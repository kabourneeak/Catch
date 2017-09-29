namespace Catch.Base
{
    public interface IHitModifier : IModifier
    {
        void OnHit(IExtendedAgent agent, AttackEventArgs e);
    }
}
