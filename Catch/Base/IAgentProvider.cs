namespace Catch.Base
{
    public interface IAgentProvider
    {
        IExtendedAgent CreateAgent(string name, CreateAgentArgs args);
    }
}