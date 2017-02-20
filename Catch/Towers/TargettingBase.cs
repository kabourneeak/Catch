using System;
using Catch.Map;
using Catch.Mobs;
using Catch.Services;

namespace Catch.Towers
{
    public abstract class TargettingBase
    {
        public abstract Tile GetBestTargetTile();

        public abstract MobBase GetBestTargetMob();

        public abstract MobBase GetBestTargetMob(Tile tile);

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
