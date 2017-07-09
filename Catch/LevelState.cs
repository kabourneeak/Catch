using System.Collections.Generic;
using Catch.Base;
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

        public List<IAgent> Agents { get; }

        public LevelState(IConfig config, MapModel map)
        {
            Config = config;
            Map = map;
            Agents = new List<IAgent>();
            Player = new PlayerModel(config);
            Ui = new UiStateModel();
        }

        public void AddAgent(IAgent agent) => Agents.Add(agent);
    }
}
