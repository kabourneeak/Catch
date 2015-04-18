using Catch.Base;

namespace Catch.Models
{
    /// <summary>
    /// A tower with no behavior or indicators whatsoever.  A Tile with this type of tower
    /// will display as empty background, not even a hexagon
    /// </summary>
    public class NilTower : Tower
    {
        public NilTower(Tile tile) : base(tile)
        {
            // no indicators
        }

        public override string GetAgentType()
        {
            return typeof (NilTower).Name;
        }

        public override void Update(float ticks)
        {
            // do nothing
        }

        public override void CreateResources(CreateResourcesArgs createArgs)
        {
            // do nothing
        }

        public override void Draw(DrawArgs drawArgs, float rotation)
        {
            // do nothing;
        }
    }
}
