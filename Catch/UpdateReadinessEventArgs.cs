using Catch.Base;

namespace Catch
{
    public class UpdateReadinessEventArgs : IUpdateReadinessEventArgs
    {
        public ISimulationState Sim { get; set; }

        public ILabelProvider LabelProvider { get; set; }
    }
}