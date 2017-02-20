using System;
using Windows.Foundation;

namespace Catch.Win2d
{
    public class PointerData : IEquatable<PointerData>
    {
        public uint PointerId { get; protected set; }
        public float X { get; protected set; }
        public float Y { get; protected set; }

        public PointerData(uint id, Point wfPoint)
        {
            PointerId = id;
            X = (float) wfPoint.X;
            Y = (float) wfPoint.Y;
        }

        public void Update(Point wfPoint) {
            X = (float)wfPoint.X;
            Y = (float)wfPoint.Y;
        }

        public bool Equals(PointerData o)
        {
            if (o == null)
            {
                return false;
            }

            return PointerId == o.PointerId && X.Equals(o.X) && Y.Equals(o.Y);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var o = (PointerData)obj;

            return PointerId == o.PointerId && X.Equals(o.X) && Y.Equals(o.Y);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return X.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}:({1},{2})", PointerId, X, Y);
        }
    }
}
