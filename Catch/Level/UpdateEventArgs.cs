using System;
using Catch.Base;

namespace Catch.Level
{
    public class UpdateEventArgs : IUpdateEventArgs
    {
        public float Ticks { get; set; }

        public ISimulationManager Manager { get; }

        public ISimulationState Sim { get; }

        public ILabelProvider LabelProvider { get; }

        public UpdateEventArgs(ISimulationManager simulationManager, ISimulationState sim, ILabelProvider labelProvider)
        {
            Manager = simulationManager ?? throw new ArgumentNullException(nameof(simulationManager));
            Sim = sim ?? throw new ArgumentNullException(nameof(sim));
            LabelProvider = labelProvider ?? throw new ArgumentNullException(nameof(labelProvider));
        }
    }
}