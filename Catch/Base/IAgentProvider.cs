using System;
using Catch.Graphics;

namespace Catch.Base
{
    public interface IAgentProvider : IGraphicsResource
    {
        IAgent CreateAgent(string name, CreateAgentArgs args);
    }
}
