namespace Catch.Base
{
    public abstract class Mob : AgentBase
    {
        protected Mob()
        {
            IsTargetable = true;
        }

        #region Mob Specific

        /// <summary>
        /// Regardless of how a mob makes its way through a tile, it spends 
        /// some amount of time in there. Considering the entrance and exit 
        /// times, the mob is some proportion of its way through the tile
        /// from 0.0 to 1.0.
        /// </summary>
        public float TileProgress { get; set; }

        #endregion
    }
}
