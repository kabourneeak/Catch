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
        /// Indicates whether this command is ready to execute. You may assume that <see cref="IsVisible"/> is true 
        /// and <see cref="Progress"/> is 1.0 if this is true, but this can be false even if those other conditions
        /// are met.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// A value from 0.0 to 1.0 indicating something about the command's availability. 
        /// e.g., how long until the command becomes available again, or how much ammo is
        /// remaining.  
        /// 
        /// This value should be set to 1.0 when <see cref="IsReady"/> is true
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// Determines the user interface available for the command.
        /// </summary>
        AgentCommandType CommandType { get; }

        /// <summary>
        /// Update the readiness state of the command, and set the properties <see cref="IsVisible"/>, 
        /// <see cref="IsReady"/>, and <see cref="Progress"/>.
        /// </summary>
        /// <returns>The value of <see cref="IsReady"/> at the conclusion of the update</returns>
        bool UpdateReadiness(IUpdateReadinessEventArgs args);

        /// <summary>
        /// Execute the command.
        /// </summary>
        void Execute(IExecuteEventArgs args);
    }

    public interface IUpdateReadinessEventArgs
    {
        ISimulationState Sim { get; }
    }

    public interface IExecuteEventArgs
    {
        ISimulationState Sim { get; }

        ISimulationManager Manager { get; }

        IAgent SelectedAgent { get; }

        ITileAgent SelectedTileAgent { get; }
    }

    public class UpdateReadinessEventArgs : IUpdateReadinessEventArgs
    {
        public ISimulationState Sim { get; set; }
    }

    public class ExecuteEventArgs : IExecuteEventArgs
    {
        public ISimulationState Sim { get; set; }

        public ISimulationManager Manager { get; set; }

        public IAgent SelectedAgent { get; set; }

        public ITileAgent SelectedTileAgent { get; set; }
    }
}
