namespace Catch.Base
{
    /// <summary>
    /// Models a single attack.  Built up by the Source agent, then applied against the Target.
    /// </summary>
    public class AttackModel
    {
        public IExtendedAgent Source { get; set; }
        public IAgent Target { get; set; }
    }
}