using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Towers
{
    public abstract class TargettingBase
    {        
        public abstract IMapTile GetBestTargetTile();

        public abstract IAgent GetBestTargetMob(IMapTile tile);

        /// <returns>The sum of the changes in AgentVersion over all tiles in the targetting 
        /// range since the last call to this method</returns>
        public abstract int GetAgentVersionDelta();

        /// <returns>The sum of the changes in TileAgentVersion over all tiles in the targetting
        /// range since the last call to this method</returns>
        public abstract int GetTileAgentVersionDelta();

        /// <summary>
        /// The team of the host using this targetting; agents on the same team will 
        /// not be targetted
        /// </summary>
        public int OwnTeam { get; set; }

        public static float ShortestRotationDirection(float fromRadians, float toRadians)
        {
            const float pi = (float) Math.PI;
            const float clockwise = -1.0f;
            const float anticlockwise = 1.0f;

            var clockwiseDist = fromRadians.Wrap(-toRadians, 0.0f, 2.0f * pi);

            return clockwiseDist <= pi ? clockwise : anticlockwise;
        }
    }
}
