﻿using System;
using Catch.Base;
using CatchLibrary.Heap;

namespace Catch
{
    /// <summary>
    /// Handles the scheduling of Updatable agents and other objects in the simulation
    /// </summary>
    public class UpdateController
    {
        private const float RegisterDelta = 0.1f;

        private readonly MinHeap<float, SchedulerEntry> _queue;
        private readonly UpdateEventArgs _updateEventArgs;
        private float _elapsedDeviceTicks;

        public event EventHandler<IUpdatable> OnRegistered;
        public event EventHandler<IUpdatable> OnDeregistered;

        public UpdateController(ISimulationManager simulationManager, ISimulationState simState)
        {
            _queue = new MinHeap<float, SchedulerEntry>();
            _updateEventArgs = new UpdateEventArgs(simulationManager, simState);
        }

        public void Update(float deviceTicks)
        {
            _elapsedDeviceTicks += deviceTicks;

            while (!_queue.IsEmpty && _queue.PeekPriority() < _elapsedDeviceTicks)
            {
                var entry = _queue.Peek();

                // update event args with info local to the task
                _updateEventArgs.Ticks = _elapsedDeviceTicks - entry.EnqueueTicks;

                // run the task
                var nextTicks = entry.Task.Update(_updateEventArgs);

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

    internal class UpdateEventArgs : IUpdateEventArgs
    {
        public float Ticks { get; set; }

        public ISimulationManager Manager { get; }

        public ISimulationState Sim { get; }

        public UpdateEventArgs(ISimulationManager simulationManager, ISimulationState sim)
        {
            Manager = simulationManager;
            Sim = sim;
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
