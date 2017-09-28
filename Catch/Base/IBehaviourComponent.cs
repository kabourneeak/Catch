namespace Catch.Base
{
    /// <summary>
    /// Implements the behaviour portions of <see cref="IExtendedAgent"/> as a plugable 
    /// module for <see cref="AgentBase"/>
    /// </summary>
    public interface IBehaviourComponent : IUpdatable
    {
        void OnRemove();

        void OnChange(AttackModel attack);
    }
}