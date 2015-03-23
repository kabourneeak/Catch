using Catch.Models;

namespace Catch.Base
{
    class BasicMob : IMovable
    {
        public float TileProgress { get; protected set; }
        public IPath Path { get; protected set; }
        public IHexTile Tile { get; protected set; }

        protected float Velocity { get; set; }
        protected int PathIndex { get; set; }

        public BasicMob(IPath path)
        {
            Velocity = (1/60.0f);

            Path = path;
            PathIndex = 0;
            Tile = path[PathIndex];
            TileProgress = 0.5f; // start in the center of our source tile
        }

        public void Update(int ticks)
        {
            // advance through tile
            TileProgress += Velocity*ticks;

            // advance to next tile, if necessary
            while (TileProgress > 1 && PathIndex < (Path.Count - 1))
            {
                PathIndex += 1;
                TileProgress -= 1.0f;
                Tile = Path[PathIndex];
            }
        }
    }
}
