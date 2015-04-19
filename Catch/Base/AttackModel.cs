namespace Catch.Base
{
    /// <summary>
    /// Models a single attack.  Build up by the Source agent, then applied against the Target.
    /// </summary>
    public class AttackModel
    {
        public IAgent Source { get; set; }
        public IAgent Target { get; set; }

        public AttackType Type { get; set; }
        public bool IsCounterAttack { get; set; }

        public int Damage { get; set; }
    }

    public enum AttackType
    {
        Normal, Reflected, OverTime, Special
    }
}