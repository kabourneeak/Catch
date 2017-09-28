using Catch.Graphics;

namespace Catch.Base
{
    public interface IAgentProvider : IGraphicsResource
    {
        IExtendedAgent CreateAgent(string name, CreateAgentArgs args);
    }
}
