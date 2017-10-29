namespace Catch.Base
{
    public interface IAgentFactory
    {
        /// <summary>
        /// Returns the type of agent that this factory produces
        /// </summary>
        string AgentType { get; }

        IExtendedAgent CreateAgent(CreateAgentArgs args);
    }
}
