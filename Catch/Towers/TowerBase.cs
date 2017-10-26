using System;
using Catch.Base;

namespace Catch.Towers
{
    /// <summary>
    /// Instantiates all of the underlying objects required for a "full" instance of a Tower
    /// 
    /// Dispatches update/create resources/draw to all child objects that require them in some sane order. Pushes
    /// and pops a translation to Draw.DrawArgs for the center point of the tower, so that all Indicators can 
    /// draw relatively.
    /// </summary>
    [Obsolete]
    public abstract class TowerBase : AgentBase, IExtendedTileAgent
    {
        protected TowerBase(string agentType, IMapTile tile) : base(agentType)
        {
            Tile = tile;
            Position = tile.Position;
        }
    }
}
