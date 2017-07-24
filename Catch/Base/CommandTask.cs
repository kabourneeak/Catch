namespace Catch.Base
{
    /// <summary>
    /// A task that will fire once as soon as it is first called to Update, 
    /// before deregistering itself  (in practice, this is very soon after
    /// it is registered).
    /// </summary>
    public abstract class CommandTask : IUpdatable
    {
        protected CommandTask()
        {
            
        }

        public float Update(IUpdateEventArgs e)
        {
            OnElapsed(e);

            return 0.0f;
        }

        protected abstract void OnElapsed(IUpdateEventArgs e);
    }
}
