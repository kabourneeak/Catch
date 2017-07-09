using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CatchLibrary.HexGrid
{
    [DataContract]
    public class HexCoords
    {
        #region Axial Coordinates

        /*
         * For flat-topped hexagons, such as ours, the columns Q are vertical,
         * while the rows R have a slanted axis.
         */
        [DataMember]
        public int Q { get; }

        [DataMember]
        public int R { get; }

        #endregion

        #region Cube Coordinates

        public int X => Q;

        public int Y => -X - Z;

        public int Z => R;

        #endregion

        #region Offset Coordinates

        /*
         * For flat-topped hexagons, such as ours, the offset column and Q are equal.
         */

        public int Row => R + (Q + (Q & 1)) / 2;

        public int Column => Q;

        #endregion

        #region Construction

        [JsonConstructor]
        public HexCoords(int q, int r)
        {
            Q = q;
            R = r;
        }

        public static HexCoords CreateFromAxial(int q, int r)
        {
            return new HexCoords(q, r);
        }

        public static HexCoords CreateFromOffset(int row, int column)
        {
            var q = column;
            var r = row - (q + (q & 1)) / 2;

            return new HexCoords(q, r);
        }

        public static HexCoords CreateFromCube(int x, int y, int z)
        {
            if (x + y + z != 0)
                throw new ArgumentOutOfRangeException(nameof(y), y, "Cubic coordinates must satisfy x + y + z == 0");

            return new HexCoords(x, z);
        }

        #endregion

        #region Equality

        public override bool Equals(object other)
        {
            return Equals(other as HexCoords);
        }

        public bool Equals(HexCoords other)
        {
            return ReferenceEquals(this, other)
                    || (!ReferenceEquals(other, null)
                        && Q == other.Q && R == other.R);
        }

        public override int GetHashCode()
        {
            return CalculateHashCode(this);
        }

        public static int CalculateHashCode(HexCoords hc)
        {
            unchecked // it is okay to overflow
            {
                var hash = 19;

                hash = hash * 31 + hc.Q;
                hash = hash * 31 + hc.R;

                return hash;
            }
        }

        public override string ToString() => $"{nameof(HexCoords)} ({Q},{R})";

        #endregion
    }
}