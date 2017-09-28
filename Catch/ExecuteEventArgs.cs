using Catch.Base;

namespace Catch
{
    public class ExecuteEventArgs : IExecuteEventArgs
    {
        public ISimulationState Sim { get; set; }

        public ISimulationManager Manager { get; set; }

        public ILabelProvider LabelProvider { get; set; }

        public IExtendedAgent SelectedAgent { get; set; }

        public IExtendedTileAgent SelectedTileAgent { get; set; }
    }
}