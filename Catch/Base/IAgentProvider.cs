using System;
using Catch.Graphics;

namespace Catch.Base
{
    public interface IAgentProvider : IGraphicsResource
    {
        IAgent CreateAgent(string name, CreateAgentArgs args);

        event EventHandler<AgentCreatedEventArgs> AgentCreated;
    }

    public class AgentCreatedEventArgs : EventArgs
    {
        public IAgent Agent { get; }

        public AgentCreatedEventArgs(IAgent agent)
        {
            Agent = agent;
        }
    }
}
