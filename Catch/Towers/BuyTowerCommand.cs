using Catch.Base;

namespace Catch.Towers
{
    public class BuyTowerCommand : IAgentCommand
    {
        private readonly IAgent _agent;
        private readonly ILevelStateModel _level;
        private readonly ISimulationManager _simulationManager;

        public BuyTowerCommand(IAgent agent, ILevelStateModel level, ISimulationManager simulationManager)
        {
            _agent = agent;
            _level = level;
            _simulationManager = simulationManager;
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

            // remove current tower
            _simulationManager.Remove(_agent);

            // create new tower
            var towerArgs = new CreateAgentArgs()
            {
                Tile = tile
            };

            var tower = _simulationManager.CreateTileAgent(nameof(GunTower), towerArgs);

            tile.TileAgent = tower;

            _simulationManager.Register(tower);

            // clear UI selections after command execution
            _level.Ui.Deselect();
        }
    }
}
