using Catch.Map;
using Catch.Services;
using CatchLibrary.HexGrid;

namespace Catch.Level
{
    /// <summary>
    /// The state of the <see cref="LevelController"/>, which is shared with its subsidiary
    /// controllers. Generally, this represents the state of a level which is "outside" of
    /// the simulation, see <see cref="SimulationStateModel"/> for part which is "inside".
    /// </summary>
    public class LevelStateModel
    {
        public IConfig Config { get; }

        public MapModel Map { get; }

        public MapTileModel OffMap { get; }

        public UiStateModel Ui { get; }

        public LevelStateModel(IConfig config, MapModel map)
        {
            Config = config;
            Map = map;
            OffMap = new MapTileModel(HexCoords.CreateFromOffset(-100, -100), config);
            Ui = new UiStateModel();
        }
    }
}
