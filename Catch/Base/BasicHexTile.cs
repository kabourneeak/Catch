using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catch.Models;

namespace Catch.Base
{
    public class BasicHexTile : IHexTile
    {
        private ITower _tower;

        public int Row { get; protected set; }
        public int Column { get; protected set; }

        public BasicHexTile(int row, int col)
        {
            Row = row;
            Column = col;
        }

        public bool HasTower()
        {
            return _tower != null;
        }

        public ITower GetTower()
        {
            return _tower;
        }

        protected void SetTower(ITower tower)
        {
            _tower = tower;
        }
    }
}
