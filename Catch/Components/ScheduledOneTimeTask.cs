using Catch.Base;

namespace Catch.Components
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

        public float Update(IUpdateEventArgs args)
        {
            _elapsedTicks += args.Ticks;

            // reschedule if schedule not elapsed
            if (_elapsedTicks < _offsetTicks)
                return _offsetTicks - _elapsedTicks;

            OnElapsed(args);
            return 0.0f;
        }

        protected abstract void OnElapsed(IUpdateEventArgs args);
    }
}
