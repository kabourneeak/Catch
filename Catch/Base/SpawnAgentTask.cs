namespace Catch.Base
{
    public class SpawnAgentTask : ScheduledOneTimeTask
    {
        private readonly string _agentName;
        private readonly CreateAgentArgs _createArgs;

        public SpawnAgentTask(float offsetTicks, string agentName, CreateAgentArgs createArgs) : base(offsetTicks)
        {
            _agentName = agentName;
            _createArgs = createArgs;
        }

        protected override void OnElapsed(IUpdateEventArgs args)
        {
            var agent = args.Manager.CreateAgent(_agentName, _createArgs);

            args.Manager.Register(agent);
        }
    }
}
