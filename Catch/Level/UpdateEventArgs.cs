using Catch.Base;

namespace Catch.Level
{
    internal class UpdateEventArgs : IUpdateEventArgs
    {
        public float Ticks { get; set; }

        public ISimulationManager Manager { get; }

        public ISimulationState Sim { get; }

        public ILabelProvider LabelProvider { get; }

        public UpdateEventArgs(ISimulationManager simulationManager, ISimulationState sim, ILabelProvider labelProvider)
        {
            Manager = simulationManager;
            Sim = sim;
            LabelProvider = labelProvider;
        }
    }
}