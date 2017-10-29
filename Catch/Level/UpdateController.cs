using System;
using Catch.Base;
using CatchLibrary.Heap;

namespace Catch.Level
{
    /// <summary>
    /// Handles the scheduling of Updatable agents and other objects in the simulation
    /// </summary>
    public class UpdateController
    {
        private const float RegisterDelta = 0.1f;

        private readonly MinHeap<float, SchedulerEntry> _queue;
        private float _elapsedDeviceTicks;

        public event EventHandler<IUpdatable> OnRegistered;
        public event EventHandler<IUpdatable> OnDeregistered;

        public UpdateController()
        {
            _queue = new MinHeap<float, SchedulerEntry>();
        }

        public void Update(float deviceTicks, UpdateEventArgs updateEventArgs)
        {
            _elapsedDeviceTicks += deviceTicks;

            while (!_queue.IsEmpty && _queue.PeekPriority() < _elapsedDeviceTicks)
            {
                var entry = _queue.Peek();

                // update event args with info local to the task
                updateEventArgs.Ticks = _elapsedDeviceTicks - entry.EnqueueTicks;

                // run the task
                var nextTicks = entry.Task.Update(updateEventArgs);

                // see if Updatable wants to be scheduled again
                if (nextTicks > 0)
                {
                    entry.EnqueueTicks = _elapsedDeviceTicks;
                    entry.DequeueTicks = _elapsedDeviceTicks + nextTicks;

                    _queue.Increase(entry.DequeueTicks);
                }
                else
                {
                    _queue.Extract(out _);

                    OnDeregistered?.Invoke(this, entry.Task);
                }
            }
        }

        public void Register(IUpdatable updatable)
        {
            var entry = new SchedulerEntry(_elapsedDeviceTicks, _elapsedDeviceTicks + RegisterDelta, updatable);
            _queue.Add(entry.DequeueTicks, entry);

            OnRegistered?.Invoke(this, updatable);
        }
    }

    internal class SchedulerEntry
    {
        public float EnqueueTicks;

        public float DequeueTicks;

        public readonly IUpdatable Task;

        public SchedulerEntry(float enqueueTicks, float dequeueTicks, IUpdatable task)
        {
            this.EnqueueTicks = enqueueTicks;
            this.DequeueTicks = dequeueTicks;
            this.Task = task;
        }
    }
}
