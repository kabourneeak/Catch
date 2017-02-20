using Catch.Base;

namespace Catch.Towers
{
    public class BuyTowerCommand : IAgentCommand
    {
        private readonly IAgent _agent;
        private readonly ILevelStateModel _level;

        public BuyTowerCommand(IAgent agent, ILevelStateModel level)
        {
            _agent = agent;
            _level = level;
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

            var tower = new GunTower(tile, _level);

            tile.AddTower(tower);

            // existing tower must go
            _agent.OnRemove();

            // new tower needs to be added to list of agents so that it is treated as part of the game
            _level.AddAgent(tower);

            // clear UI selections after command execution
            _level.Ui.Deselect();
        }
    }
}
