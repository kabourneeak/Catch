namespace Catch.Base
{
    /// <summary>
    /// AgentStats modifiers set the <see cref="IAgent.Stats"/> and <see cref="IAgent.Labels"/>
    /// for an agent.
    /// </summary>
    public interface IAgentStatsModifier : IModifier
    {
        void OnCalculateAgentStats(IExtendedAgent agent);
    }
}