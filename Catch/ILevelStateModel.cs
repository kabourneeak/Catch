﻿using Catch.Services;

namespace Catch
{
    /// <summary>
    /// Maintains the overall state of the level being played, which the UI 
    /// and Agents (and their behaviours and commands) use to inspect the
    /// simulation
    /// </summary>
    public interface ILevelStateModel
    {
        IConfig Config { get; }

        Map.Map Map { get; }

        UiStateModel Ui { get; }

        PlayerModel Player { get; }
    }
}