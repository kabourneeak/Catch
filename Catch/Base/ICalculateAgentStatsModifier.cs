namespace Catch.Base
{
    public interface ICalculateAgentStatsModifier : IModifier
    {
        void OnCalculateAgentStats(IExtendedAgent agent);
    }
}