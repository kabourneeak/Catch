using System;
using Catch.Base;

namespace Catch.Models
{
    public class BasicHexTile : IHexTile
    {
        private ITower _tower;

        public BasicHexTile(int row, int col)
        {
            Row = row;
            Column = col;
        }

        /*
         * IHexTile Implementation
         */
        public int Row { get; protected set; }
        public int Column { get; protected set; }

        public void Update(float ticks)
        {
            throw new NotImplementedException();
        }

        public IGraphicsComponent Graphics { get; set; }
        public string DisplayName { get; private set; }
        public string DisplayInfo { get; private set; }
        public string DisplayStatus { get; private set; }

        public ITower GetTower()
        {
            return _tower;
        }

        /*
         * Other 
         */
        public bool HasTower()
        {
            return _tower != null;
        }

        protected void SetTower(ITower tower)
        {
            _tower = tower;
        }
    }
}
