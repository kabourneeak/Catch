namespace Catch.Base
{
    /// <summary>
    /// An object which moves through a grid over time.
    /// </summary>
    public interface IMovable : IUpdatable
    {
        /// <summary>
        /// The path the object follows
        /// </summary>
        IPath Path { get; }

        /// <summary>
        /// The tile within the objects Path which the object currently resides
        /// </summary>
        IHexTile Tile { get; }

        /// <summary>
        /// How far through the tile the object has travelled.
        /// </summary>
        float TileProgress { get; }
    }
}
