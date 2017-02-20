using System.Collections.Generic;
using Catch.Base;
using Catch.Services;

namespace Catch
{

    /// <summary>
    /// Implementation of ILevelStateModel for LevelController
    /// </summary>
    public class LevelState : ILevelStateModel
    {
        public IConfig Config { get; }

        public Map.Map Map { get;
            // TODO get rid of this setter
            set; }

        public UiStateModel Ui { get; }

        public PlayerModel Player { get; }

        public List<IAgent> Agents { get; }

        public LevelState(IConfig config)
        {
            Config = config;

            Agents = new List<IAgent>();
            Player = new PlayerModel(config);
            Ui = new UiStateModel();
        }

        public void AddAgent(IAgent agent) => Agents.Add(agent);
    }
}
