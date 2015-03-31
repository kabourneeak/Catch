using Catch.Base;

namespace Catch.Models
{
    public abstract class PathMobAgent : IAgent
    {
        protected PathMobAgent(IPath path, IBehaviourComponent brain, IGraphicsComponent graphics)
        {
            Brain = brain;
            Graphics = graphics;
            Path = path;

            InitMobOnPath();
        }

        private void InitMobOnPath()
        {
            Velocity = (1 / 60.0f);
            PathIndex = 0;
            Tile = Path[PathIndex];
            TileProgress = 0.5f; // start in the center of our source tile
        }

        #region PathMobAgent Properties

        public float TileProgress { get; protected set; }

        public float Velocity { get; set; }

        public IPath Path { get; protected set; }

        public IHexTile Tile { get; protected set; }

        public int PathIndex { get; set; }

        #endregion

        #region IAgent Implementation

        public abstract string GetAgentType();

        public void Update(float ticks)
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

            Brain.Update(ticks);
        }

        public IBehaviourComponent Brain { get; protected set; }

        public IGraphicsComponent Graphics { get; protected set; }

        public string DisplayName { get; protected set; }

        public string DisplayInfo { get; protected set; }

        public string DisplayStatus { get; protected set; }

        public bool IsTargetable { get; protected set; }

        public int Health { get; protected set; }

        public IModifierCollection Modifiers { get; protected set; }

        public IIndicatorCollection Indicators { get; protected set; }

        public AttackSpecs AttackSpecs { get; protected set; }

        public DefenceSpecs DefenceSpecs { get; protected set; }

        public IAgentStats Stats { get; protected set; }

        #endregion
    }
}
