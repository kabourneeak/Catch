using Catch.Base;

namespace Catch
{
    public class ExecuteEventArgs : IExecuteEventArgs
    {
        public ISimulationState Sim { get; set; }

        public ISimulationManager Manager { get; set; }

        public ILabelProvider LabelProvider { get; set; }

        public IAgent SelectedAgent { get; set; }

        public IAgent SelectedTileAgent { get; set; }
    }
}