using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Level
{
    public class SimulationStateModel : ISimulationState
    {
        public IConfig Config { get; }

        public IMap Map { get; }

        public IMapTile OffMap { get; }

        public PlayerModel Player { get; }

        public SimulationStateModel(IConfig config, IMap map, IMapTile offMapTile)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Map = map ?? throw new ArgumentNullException(nameof(map));
            OffMap = offMapTile ?? throw new ArgumentNullException(nameof(offMapTile));

            Player = new PlayerModel(config.GetInt(CoreConfig.PlayerTeam));
        }
    }
}
