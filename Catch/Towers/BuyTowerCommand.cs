using Catch.Base;

namespace Catch.Towers
{
    public class BuyTowerCommand : IAgentCommand
    {
        private readonly IExtendedAgent _agent;

        public BuyTowerCommand(IExtendedAgent agent)
        {
            _agent = agent;
        }

        public string DisplayName => "Buy Gun Tower";
        public string DisplayDesc => "Places a gun tower here";
        public bool IsVisible => true;
        public bool IsReady => true;
        public float Progress => 1.0f;
        public AgentCommandType CommandType => AgentCommandType.Action;

        public bool UpdateReadiness(IUpdateReadinessEventArgs args)
        {
            // nothing to check
            return IsReady;
        }

        public void Execute(IExecuteEventArgs args)
        {
            var tile = _agent.Tile;

            // remove current tower
            args.Manager.Remove(_agent);

            // create new tower
            var towerArgs = new CreateAgentArgs()
            {
                Tile = tile
            };

            var tower = args.Manager.CreateAgent(GunTowerBehaviour.AgentTypeName, towerArgs);

            args.Manager.Register(tower);
            args.Manager.Site(tower);
        }
    }
}
