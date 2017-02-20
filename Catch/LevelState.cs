using Catch.Services;

namespace Catch
{

    /// <summary>
    /// Implementation of ILevelStateModel for LevelController
    /// </summary>
    public class LevelState : ILevelStateModel
    {
        public IConfig Config { get; private set; }

        public Map.Map Map { get; set; }

        public UiStateModel Ui { get; private set; }

        public PlayerModel Player { get; private set; }

        public LevelState(IConfig config)
        {
            Config = config;

            Player = new PlayerModel(config);
            Ui = new UiStateModel();
        }
    }
}
