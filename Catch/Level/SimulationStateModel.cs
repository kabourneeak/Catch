using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Level
{
    public class SimulationStateModel : ISimulationState
    {
        public IConfig Config { get; }

        public IMap Map { get; }

        public PlayerModel Player { get; }

        public SimulationStateModel(IConfig config, IMap map)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Map = map ?? throw new ArgumentNullException(nameof(map));
  
            Player = new PlayerModel(config.GetInt(CoreConfig.PlayerTeam));
        }
    }
}
