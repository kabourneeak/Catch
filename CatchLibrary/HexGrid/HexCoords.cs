using System;

namespace CatchLibrary.HexGrid
{
    public class HexCoords
    {
        public int Row;
        public int Column;

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

                hash = hash * 31 + hc.Row;
                hash = hash * 31 + hc.Column;

                return hash;
            }
        }

        #endregion
    }
}