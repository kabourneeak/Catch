using Catch.Base;

namespace Catch.Towers
{
    public class BuyTowerCommand : IAgentCommand
    {
        private readonly IAgent _agent;

        public BuyTowerCommand(IAgent agent)
        {
            _agent = agent;
        }

        public string DisplayName => "Buy Gun Tower";
        public string DisplayDesc => "Places a gun tower here";
        public bool IsVisible => true;
        public bool IsReady => true;
        public float Progress => 1.0f;
        public AgentCommandType CommandType => AgentCommandType.Action;

        public bool UpdateReadiness(IUpdateReadinessEventArgs e)
        {
            // nothing to check
            return IsReady;
        }

        public void Execute(IExecuteEventArgs e)
        {
            var tile = _agent.Tile;

            // remove current tower
            e.Manager.Remove(_agent);

            // create new tower
            var towerArgs = new CreateAgentArgs()
            {
                Tile = tile
            };

            var tower = e.Manager.CreateTileAgent(nameof(GunTower), towerArgs);

            e.Manager.Register(tower);
        }
    }
}
