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

        protected override void OnElapsed(IUpdateEventArgs e)
        {
            var agent = e.Manager.CreateAgent(_agentName, _createArgs);

            e.Manager.Register(agent);
        }
    }
}
