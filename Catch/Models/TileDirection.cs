using System;
using System.Collections.Generic;
using System.Linq;

namespace Catch.Models
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