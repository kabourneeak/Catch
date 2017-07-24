namespace Catch.Base
{
    /// <summary>
    /// A task that will fire once after a given amount of time has elapsed, before
    /// deregistering itself
    /// </summary>
    public abstract class ScheduledOneTimeTask : IUpdatable
    {
        private readonly float _offsetTicks;
        private float _elapsedTicks;

        protected ScheduledOneTimeTask(float offsetTicks)
        {
            _offsetTicks = offsetTicks;
        }

        public float Update(IUpdateEventArgs e)
        {
            _elapsedTicks += e.Ticks;

            // reschedule if schedule not elapsed
            if (_elapsedTicks < _offsetTicks)
                return _offsetTicks - _elapsedTicks;

            OnElapsed(e);
            return 0.0f;
        }

        protected abstract void OnElapsed(IUpdateEventArgs e);
    }
}
