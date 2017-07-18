using Catch.Base;
using Catch.Graphics;
using Catch.Map;

namespace Catch.Towers
{
    /// <summary>
    /// Instantiates all of the underlying objects required for a "full" instance of a Tower
    /// 
    /// Dispatches update/create resources/draw to all child objects that require them in some sane order. Pushes
    /// and pops a translation to Draw.DrawArgs for the center point of the tower, so that all Indicators can 
    /// draw relatively.
    /// </summary>
    public abstract class TowerBase : AgentBase, ITileAgent
    {
        public float Rotation { get; set; }

        protected TowerBase(string agentType, Tile tile, ILevelStateModel level) : base(agentType, level)
        {
            Tile = tile;
            Position = tile.Position;

            // site into tile
            tile.AddTower(this);
        }

        public override void Draw(DrawArgs drawArgs, float rotation)
        {
            if (Indicators.Count == 0)
                return;

            drawArgs.PushTranslation(Position);

            // ignore the rotation parameter, and replace by our Rotation
            Indicators.Draw(drawArgs, Rotation);

            drawArgs.Pop();
        }
    }
}
