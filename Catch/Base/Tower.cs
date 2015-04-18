namespace Catch.Base
{
    /// <summary>
    /// Instantiates all of the underlying objects required for a "full" instance of a Tower
    /// 
    /// Dispatches update/create resources/draw to all child objects that require them in some sane order. Pushes
    /// and pops a translation to Draw.DrawArgs for the center point of the tower, so that all Indicators can 
    /// draw relatively.
    /// </summary>
    public abstract class Tower : AgentBase
    {
        protected Tower(Tile tile)
        {
            Tile = tile;
            Position = tile.Position;
            Layer = DrawLayer.Tower;

            // site into tile
            tile.AddTower(this);
        }
    }
}
