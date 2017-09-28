namespace Catch.Base
{
    public interface IExecuteEventArgs
    {
        ISimulationState Sim { get; }

        ISimulationManager Manager { get; }

        ILabelProvider LabelProvider { get; }

        IExtendedAgent SelectedAgent { get; }

        IExtendedTileAgent SelectedTileAgent { get; }
    }
}