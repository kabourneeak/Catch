using System;

namespace CatchLibrary.HexGrid
{
    public class HexCoords
    {
        #region Axial Coordinates

        /*
         * For flat-topped hexagons, such as ours, the columns Q are vertical,
         * while the rows are axial.
         */

        public int Q { get; set; }

        public int R { get; set; }

        #endregion

        #region Cubic Coordinates

        public int X => Q;

        public int Y => -X - Z;

        public int Z => R;

        #endregion

        #region Offset Coordinates

        /*
         * For flat-topped hexagons, such as ours, the offset column and Q are equal.
         */

        public int Row
        {
            get { return R + (Q + (Q & 1)) / 2; }
            set { R = value - (Q + (Q & 1)) / 2; }
        }

        public int Column
        {
            get { return Q; }
            set { Q = value; }
        }

        #endregion

        #region Construction

        private HexCoords()
        {
            
        }

        public static HexCoords CreateFromAxial(int q, int r)
        {
            var hc = new HexCoords
            {
                Q = q,
                R = r
            };

            return hc;
        }

        public static HexCoords CreateFromOffset(int row, int column)
        {
            var hc = new HexCoords();

            // order is important, since Row requires Column for its calculation
            hc.Column = column;
            hc.Row = row;

            return hc;
        }

        #endregion

        #region Equality

        public override bool Equals(object other)
        {
            return Equals(other as HexCoords);
        }

        public bool Equals(HexCoords other)
        {
            return object.ReferenceEquals(this, other)
                    || (!object.ReferenceEquals(other, null)
                        && this.Row == other.Row && this.Column == other.Column);
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

        #endregion
    }
}