namespace Catch.Base
{
    public interface IExecuteEventArgs
    {
        ISimulationState Sim { get; }

        ISimulationManager Manager { get; }

        ILabelProvider LabelProvider { get; }

        IAgent SelectedAgent { get; }

        ITileAgent SelectedTileAgent { get; }
    }
}