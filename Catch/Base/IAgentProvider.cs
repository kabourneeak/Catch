namespace Catch.Base
{
    public interface IAgentProvider
    {
        IAgent CreateAgent(string name);

        IPathAgent CreatePathAgent(string name, IPath path);

        IModifier CreateModifier(string name);

        IIndicator CreateIndicator(string name);
    }
}
