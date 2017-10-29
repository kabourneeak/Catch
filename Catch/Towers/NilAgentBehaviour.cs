using Catch.Base;

namespace Catch.Towers
{
    /// <summary>
    /// An implementation of IUpdatable that doesn't do anything
    /// </summary>
    public class NilAgentBehaviour : IUpdatable
    {
        public float Update(IUpdateEventArgs args)
        {
            // indicate that we do not require any further updates
            return 0.0f;
        }
    }
}
