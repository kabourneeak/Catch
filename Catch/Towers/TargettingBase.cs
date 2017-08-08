using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Towers
{
    public abstract class TargettingBase
    {
        public abstract IMapTile GetBestTargetTile();

        public abstract IAgent GetBestTargetMob();

        public abstract IAgent GetBestTargetMob(IMapTile tile);

        public static float ShortestRotationDirection(float fromRadians, float toRadians)
        {
            const float pi = (float) Math.PI;
            const float clockwise = -1.0f;
            const float anticlockwise = 1.0f;

            var clockwiseDist = fromRadians.Wrap(-toRadians, 0, 2*pi);

            return clockwiseDist < pi ? clockwise : anticlockwise;
        }
    }
}
