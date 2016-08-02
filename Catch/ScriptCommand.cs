namespace Catch
{
    public class ScriptCommand
    {
        /// <summary>
        /// The time, in ticks, to execute this command
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// The path to set the agents upon
        /// </summary>
        public string PathName { get; set; }

        /// <summary>
        /// The type of agent to emit
        /// </summary>
        public string AgentTypeName { get; set; }
    }
}