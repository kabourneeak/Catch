using Catch.Base;

namespace Catch.Towers
{
    public class BuyTowerCommand : IAgentCommand
    {
        private readonly IAgent _agent;
        private readonly ILevelStateModel _level;
        private readonly IAgentProvider _agentProvider;

        public BuyTowerCommand(IAgent agent, ILevelStateModel level, IAgentProvider agentProvider)
        {
            _agent = agent;
            _level = level;
            _agentProvider = agentProvider;
        }

        public void Update(float ticks)
        {
            // do nothing
        }

        public string DisplayName => "Buy Gun Tower";
        public string DisplayDesc => "Places a gun tower here";
        public bool IsVisible => true;
        public bool IsAvailable => true;
        public float Progress => 1.0f;
        public AgentCommandType CommandType => AgentCommandType.Action;

        public void Execute()
        {
            var tile = _agent.Tile;

            tile.RemoveTower((TowerBase)_agent);

            var towerArgs = new CreateAgentArgs()
            {
                StateModel = _level,
                Tile = tile
            };

            var tower = _agentProvider.CreateAgent(nameof(GunTower), towerArgs);

            tile.AddTower((ITileAgent)tower);

            // existing tower must go
            _agent.OnRemove();

            // new tower needs to be added to list of agents so that it is treated as part of the game
            _level.AddAgent(tower);

            // clear UI selections after command execution
            _level.Ui.Deselect();
        }
    }
}
