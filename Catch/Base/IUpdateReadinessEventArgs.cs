namespace Catch.Base
{
    public interface IUpdateReadinessEventArgs
    {
        ISimulationState Sim { get; }

        ILabelProvider LabelProvider { get; }
    }
}