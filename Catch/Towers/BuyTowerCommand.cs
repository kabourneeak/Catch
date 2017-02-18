using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catch.Base;
using Catch.Services;

namespace Catch.Towers
{
    public class BuyTowerCommand : IAgentCommand
    {
        private readonly IAgent _agent;
        private readonly PlayerModel _player;
        private readonly IConfig _config;

        public BuyTowerCommand(IAgent agent, PlayerModel player, IConfig config)
        {
            _agent = agent;
            _player = player;
            _config = config;
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

            var tower = new GunTower(tile, _config);

            tile.AddTower(tower);

            // existing tower must go
            _agent.OnRemove();

            // new tower needs to be added to list of agents so that it is treated as part of the game
        }
    }
}
