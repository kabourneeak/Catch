using Catch.Base;
using Catch.Map;
using Catch.Services;

namespace Catch
{
    /// <summary>
    /// Maintains the overall state of the level being played, which the UI 
    /// and Agents (and their behaviours and commands) use to inspect the
    /// simulation.
    /// 
    /// A small number of method for modifying these collections is provided
    /// </summary>
    public interface ILevelStateModel
    {
        IConfig Config { get; }

        MapModel Map { get; }

        UiStateModel Ui { get; }

        PlayerModel Player { get; }
    }
}
