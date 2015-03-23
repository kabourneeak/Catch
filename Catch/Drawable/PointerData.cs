using System;
using Windows.Foundation;

namespace Catch.Drawable
{
    class PointerData : IEquatable<PointerData>
    {
        public uint PointerId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }


        public PointerData(uint id, Point wfPoint)
        {
            PointerId = id;
            X = (float) wfPoint.X;
            Y = (float) wfPoint.Y;
        }

        public void update(Point wfPoint) {
            X = (float)wfPoint.X;
            Y = (float)wfPoint.Y;
        }

        public bool Equals(PointerData o)
        {
            if (o == null)
            {
                return false;
            }

            return (PointerId == o.PointerId && X == o.X && Y == o.Y);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PointerData o = (PointerData)obj;

            return (PointerId == o.PointerId && X == o.X && Y == o.Y);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return X.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0}:({1},{2})", PointerId, X, Y);
        }
    }
}
