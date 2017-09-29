namespace Catch.Base
{
    /// <summary>
    /// Models a single attack.  Built up by the Source agent, then applied against the Target.
    /// </summary>
    public class AttackEventArgs
    {
        /// <summary>
        /// The agent originating the attack
        /// </summary>
        public IAgent Source { get; }

        /// <summary>
        /// The agent under attack
        /// </summary>
        public IAgent Target { get; }

        /// <summary>
        /// Changes to be applied to the stats of the target
        /// </summary>
        public BaseStatsModel StatsDelta { get; }

        public AttackEventArgs(IAgent source, IAgent target)
        {
            Source = source;
            Target = target;
            StatsDelta = new BaseStatsModel();
        }
    }
}