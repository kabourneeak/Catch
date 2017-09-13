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

        /// <summary>
        /// A place which is off the map that agents can exist when not otherwise placed
        /// </summary>
        IMapTile OffMap { get; }

        PlayerModel Player { get; }
    }
}
