using Catch.Base;
using Catch.Services;

namespace Catch
{
    public class SimulationStateModel : ISimulationState
    {
        public IConfig Config { get; }

        public IMap Map { get; }

        public PlayerModel Player { get; }

        public SimulationStateModel(IConfig config, IMap map)
        {
            Map = map;
            Config = config;
            Player = new PlayerModel();
        }
    }
}
