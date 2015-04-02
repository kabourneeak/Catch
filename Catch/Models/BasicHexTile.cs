using System;
using System.Numerics;
using Windows.UI;
using Catch.Base;
using Catch.Services;
using Catch.Win2d;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class BasicHexTile : IHexTile
    {
        private ITower _tower;
        private float _radius;
        private float _radiusH;

        public BasicHexTile(int row, int col, IConfig config)
        {
            Row = row;
            Column = col;

            Layer = DrawLayer.Base;

            // copy down config
            _radius = config.GetFloat("TileRadius");

            _radiusH = HexUtils.GetRadiusHeight(_radius);

            var x = _radius + (col * (_radius + _radius * HexUtils.COS60));
            var y = (col % 2 * _radiusH) + (row * 2 * _radiusH) + _radiusH;

            Position = new Vector2(x, y);

            // create sub-objects
            Graphics = new BasicHexTileGraphics(this, _radius);
        }

        #region BasicHexTile Implementation

        public IGraphicsComponent Graphics { get; protected set; }

        public bool HasTower()
        {
            return _tower != null;
        }
        
        protected void SetTower(ITower tower)
        {
            _tower = tower;
        }

        #endregion

        #region IHexTile Implementation

        public int Row { get; protected set; }

        public int Column { get; protected set; }

        public ITower GetTower()
        {
            return _tower;
        }

        #endregion

        #region IGameObject Implementation

        public string DisplayName { get; protected set; }

        public string DisplayInfo { get; protected set; }

        public string DisplayStatus { get; protected set; }

        public Vector2 Position { get; protected set; }

        public DrawLayer Layer { get; protected set; }

        public void Update(float ticks)
        {
            Graphics.Update(ticks);
        }

        public void CreateResources(CanvasDrawingSession ds)
        {
            Graphics.CreateResources(ds);
        }

        public void Draw(CanvasDrawingSession ds)
        {
            Graphics.Draw(ds);
        }

        #endregion
    }
}
