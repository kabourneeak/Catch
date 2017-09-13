using Catch.Base;
using Catch.Services;

namespace Catch
{
    public class SimulationStateModel : ISimulationState
    {
        public IConfig Config { get; }

        public IMap Map { get; }

        public IMapTile OffMap { get; }

        public PlayerModel Player { get; }

        public SimulationStateModel(IConfig config, IMap map, IMapTile offMapTile)
        {
            Map = map;
            Config = config;
            OffMap = offMapTile;
            Player = new PlayerModel();
        }
    }
}
