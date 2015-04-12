using System.Numerics;
using Catch.Services;

namespace Catch.Base
{
    public class Tile
    {
        private Tower _tower;

        public Tile(int row, int col, IConfig config)
        {
            Row = row;
            Column = col;

            // copy down config
            var radius = config.GetFloat("TileRadius");

            // calculate position
            var radiusH = HexUtils.GetRadiusHeight(radius);

            var x = radius + (col * (radius + radius * HexUtils.COS60));
            var y = (col % 2 * radiusH) + (row * 2 * radiusH) + radiusH;

            Position = new Vector2(x, y);
        }

        #region Tile Implementation

        public int Row { get; protected set; }

        public int Column { get; protected set; }

        public Vector2 Position { get; protected set; }

        public Tower GetTower()
        {
            return _tower;
        }

        public bool HasTower()
        {
            return _tower != null;
        }

        public void SetTower(Tower tower)
        {
            _tower = tower;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Tile {0},{1}", Row, Column);
        }
    }
}
