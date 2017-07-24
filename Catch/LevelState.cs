using Catch.Map;
using Catch.Services;

namespace Catch
{

    /// <summary>
    /// Implementation of ILevelStateModel for LevelController
    /// </summary>
    public class LevelState : ILevelStateModel
    {
        public IConfig Config { get; }

        public MapModel Map { get; }

        public UiStateModel Ui { get; }

        public PlayerModel Player { get; }

        public LevelState(IConfig config, MapModel map)
        {
            Config = config;
            Map = map;
            Player = new PlayerModel(config);
            Ui = new UiStateModel();
        }
    }
}
