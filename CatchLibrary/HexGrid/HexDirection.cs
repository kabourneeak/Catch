using System;
using System.Collections.Generic;

namespace CatchLibrary.HexGrid
{
    public enum HexDirection
    {
        North,
        NorthWest,
        SouthWest,
        South,
        SouthEast,
        NorthEast
    }

    public static class HexDirectionExtensions
    {
        private static readonly Random _rng = new Random();

        private static readonly List<HexDirection> _allDirections = new List<HexDirection>() {HexDirection.North, 
            HexDirection.NorthWest, HexDirection.SouthWest, HexDirection.South, HexDirection.SouthEast, HexDirection.NorthEast};

        private const int numDirections = 6;
        
        private const float pi = (float)Math.PI;

        public static IEnumerable<HexDirection> AllDirections => _allDirections;

        public static HexDirection GetRandom()
        {
            return _allDirections[_rng.Next(numDirections)];
        }

        public static float CenterRadians(this HexDirection hexDirection)
        {
            switch (hexDirection)
            {
                case HexDirection.North:
                    return 3 * pi / 6;
                case HexDirection.NorthWest:
                    return 5 * pi / 6;
                case HexDirection.SouthWest:
                    return 7 * pi / 6;
                case HexDirection.South:
                    return 9 * pi / 6;
                case HexDirection.SouthEast:
                    return 11 * pi / 6;
                case HexDirection.NorthEast:
                    return 1 * pi / 6;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static int Ordinal(this HexDirection d)
        {
            return _allDirections.IndexOf(d);
        }

        /// <summary>
        /// Given a starting and destination hex direction, returns a multiplier you can 
        /// apply to a radian velocity to rotate in the shortest direction to your target.
        /// </summary>
        /// <param name="from">The direction you are starting from</param>
        /// <param name="to">The direction you are heading to</param>
        /// <returns>-1.0f if clockwise is the shortest rotation to reach to, 1.0f otherwise. 
        /// One of these is chosen randomly if from and to are opposites (e.g., north and 
        /// south)</returns>
        public static float ShortestRotationDirection(HexDirection from, HexDirection to)
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