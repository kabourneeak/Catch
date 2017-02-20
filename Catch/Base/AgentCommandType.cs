namespace Catch.Base
{
    public enum AgentCommandType
    {
        /// <summary>
        /// The command should complete immediately upon invokation
        /// </summary>
        Action,

        /// <summary>
        /// The command should prompt for confirmation before invoking
        /// </summary>
        Confirm,

        /// <summary>
        /// The command presents a list of sub options, one of which must be chosen for invokation
        /// </summary>
        List,

        /// <summary>
        /// The player must select a tower to complete the invokation. If the agent exposing the command
        /// is a tower, then a different tower must be selected.
        /// </summary>
        SelectTower,

        /// <summary>
        /// The player must select a mob to complete the invokation. if the agent exposing the command 
        /// is a mob, then a different mob must be selected.
        /// </summary>
        SelectMob
    }
}