using System;

namespace Catch.Base
{
    public abstract class Targetting
    {
        public abstract Tile GetBestTargetTile();

        public abstract Mob GetBestTargetMob();

        public abstract Mob GetBestTargetMob(Tile tile);

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
