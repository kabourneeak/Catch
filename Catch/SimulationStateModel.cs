using Catch.Base;
using Catch.Map;
using Catch.Services;

namespace Catch
{
    public class SimulationStateModel : ISimulationState
    {
        public IConfig Config { get; set; }
        public PlayerModel Player { get; set; }
        public MapModel Map { get; set; }
    }
}
