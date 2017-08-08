using Catch.Services;

namespace Catch.Base
{
    /// <summary>
    /// The state of the simulation, which represents all which is observable from "inside"
    /// the simulation.
    /// </summary>
    public interface ISimulationState
    {
        IConfig Config { get; }

        IMap Map { get; }

        PlayerModel Player { get; }
    }
}
