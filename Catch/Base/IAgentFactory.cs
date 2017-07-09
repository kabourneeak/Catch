using Catch.Graphics;

namespace Catch.Base
{
    public interface IAgentFactory : IGraphicsResource
    {
        /// <summary>
        /// Returns the type of agent that this factory produces
        /// </summary>
        string AgentType { get; }

        IAgent CreateAgent(CreateAgentArgs args);
    }
}
