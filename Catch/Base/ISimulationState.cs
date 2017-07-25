using Catch.Map;
using Catch.Services;

namespace Catch.Base
{
    public interface ISimulationState
    {
        IConfig Config { get; }

        PlayerModel Player { get; }

        MapModel Map { get; }
    }
}
