namespace Catch.Base
{
    public interface IPathAgent : IAgent
    {
        IPath Path { get; }

        int PathIndex { get; set; }

        float TileProgress { get; }

        float Velocity { get; }
    }
}
