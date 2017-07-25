using Catch.Map;
using Catch.Services;

namespace Catch
{
    /// <summary>
    /// The state of the <see cref="LevelController"/>, which is shared with its subsidiary
    /// controllers.
    /// </summary>
    public class LevelStateModel
    {
        public IConfig Config { get; }

        public MapModel Map { get; }

        public UiStateModel Ui { get; }

        public LevelStateModel(IConfig config, MapModel map)
        {
            Config = config;
            Map = map;
            Ui = new UiStateModel();
        }
    }
}
