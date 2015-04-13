using System;
using System.Collections.Generic;

namespace Catch.Base
{
    public enum TileDirection
    {
        North,
        NorthWest,
        SouthWest,
        South,
        SouthEast,
        NorthEast
    }

    public static class TileDirectionExtensions
    {
        private static readonly Random _rng = new Random();

        private static readonly List<TileDirection> _allDirections = new List<TileDirection>() {TileDirection.North, 
            TileDirection.NorthWest, TileDirection.SouthWest, TileDirection.South, TileDirection.SouthEast, TileDirection.NorthEast};

        private const int numDirections = 6;
        
        private const float pi = (float)Math.PI;

        public static IEnumerable<TileDirection> AllTileDirections {get { return _allDirections; } }

        public static TileDirection GetRandom()
        {
            return _allDirections[_rng.Next(numDirections)];
        }

        public static float CenterRadians(this TileDirection tileDirection)
        {
            switch (tileDirection)
            {
                case TileDirection.North:
                    return 3 * pi / 6;
                case TileDirection.NorthWest:
                    return 5 * pi / 6;
                case TileDirection.SouthWest:
                    return 7 * pi / 6;
                case TileDirection.South:
                    return 9 * pi / 6;
                case TileDirection.SouthEast:
                    return 11 * pi / 6;
                case TileDirection.NorthEast:
                    return 1 * pi / 6;
                default:
                    throw new ArgumentException("Missing enumeration");
            }
        }

        private static int Ordinal(this TileDirection d)
        {
            return _allDirections.IndexOf(d);
        }

        /// <summary>
        /// Given a starting and destination tile direction, returns a multiplier you can 
        /// apply to a radian velocity to rotate in the shortest direction to your target.
        /// </summary>
        /// <param name="from">The direction you are starting from</param>
        /// <param name="to">The direction you are heading to</param>
        /// <returns>-1.0f if clockwise is the shortest rotation to reach to, 1.0f otherwise. 
        /// One of these is chosen randomly if from and to are opposites (e.g., north and 
        /// south)</returns>
        public static float ShortestRotationDirection(TileDirection from, TileDirection to)
        {
            const float clockwise = -1.0f;
            const float anticlockwise = 1.0f;

            var clockwiseDist = (from.Ordinal() - to.Ordinal()).Mod(6);

            if (clockwiseDist < 3)
            {
                return clockwise;
            }
            else if (clockwiseDist == 3)
            {
                return (_rng.Next(2) == 0) ? clockwise : anticlockwise;
            }
            else
            {
                return anticlockwise;
            }
        }
    }
}