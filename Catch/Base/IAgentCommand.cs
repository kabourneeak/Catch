namespace Catch.Base
{
    public interface IAgentCommand
    {
        /// <summary>
        /// The short, player-friendly name for the command. e.g., "Upgrade".
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The longer player-friendly description of the command. 
        /// e.g., "Upgrades this tower to Level 2".
        /// </summary>
        string DisplayDesc { get; }

        /// <summary>
        /// Indicates whether this command should be presented to the player.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Indicates whether this command is ready to execute.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// A value from 0.0 to 1.0 indicating something about the command's availability. 
        /// e.g., how long until the command becomes available again, or how much ammo is
        /// remaining.  
        /// 
        /// This value should be set to 1.0 when there is nothing interesting
        /// to show the player.
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// Determines the user interface available for the command.
        /// </summary>
        AgentCommandType CommandType { get; }

        /// <summary>
        /// Execute the command.
        /// </summary>
        void Execute(IExecuteEventArgs e);
    }

    public interface IExecuteEventArgs
    {
        ILevelStateModel LevelState { get; }

        ISimulationManager Manager { get; }

        IAgent SelectedAgent { get; }

        ITileAgent SelectedTileAgent { get; }
    }

    public class ExecuteEventArgs : IExecuteEventArgs
    {
        public ILevelStateModel LevelState { set; get; }

        public ISimulationManager Manager { set; get; }

        public IAgent SelectedAgent { set; get; }

        public ITileAgent SelectedTileAgent { set; get; }
    }
}
