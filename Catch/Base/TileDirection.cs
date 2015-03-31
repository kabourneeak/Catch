using System.Collections.Generic;

namespace Catch.Base
{
    public enum TileDirection
    {
        North, NorthEast, SouthEast, South, SouthWest, NorthWest
    }

    public static class TileDirectionExtensions
    {
        public static readonly IEnumerable<TileDirection> AllTileDirections = new List<TileDirection>() {TileDirection.North, 
            TileDirection.NorthEast, TileDirection.SouthEast, TileDirection.South, TileDirection.SouthWest, TileDirection.NorthWest};
    }
}