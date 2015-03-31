namespace Catch.Base
{
    public interface IAgentProvider
    {
        IAgent CreateAgent(string name);

        IModifier CreateModifier(string name);

        IIndicator CreateIndicator(string name);
    }
}
