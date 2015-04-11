using System.Numerics;
using Windows.UI;
using Catch.Models;
using Catch.Services;

namespace Catch.Base
{
    public class Tile : IGameObject
    {
        private Tower _tower;
        private float _radius;
        private float _radiusH;

        public Tile(int row, int col, IConfig config)
        {
            Row = row;
            Column = col;

            Layer = DrawLayer.Base;

            // copy down config
            _radius = config.GetFloat("TileRadius");

            // calculate position
            _radiusH = HexUtils.GetRadiusHeight(_radius);

            var x = _radius + (col * (_radius + _radius * HexUtils.COS60));
            var y = (col % 2 * _radiusH) + (row * 2 * _radiusH) + _radiusH;

            Position = new Vector2(x, y);

            // create sub-objects
            var style = new StyleArgs() {BrushType = BrushType.Solid, Color = Colors.DarkRed, StrokeWidth = 4};
            Graphics = new HexagonGraphics(Position, _radius, style);
        }

        #region Tile Implementation

        public IGraphicsComponent Graphics { get; protected set; }

        public int Row { get; protected set; }

        public int Column { get; protected set; }

        public Tower GetTower()
        {
            return _tower;
        }

        public bool HasTower()
        {
            return _tower != null;
        }

        protected void SetTower(Tower tower)
        {
            _tower = tower;
        }

        #endregion

        #region IGameObject Implementation

        public string DisplayName { get; protected set; }

        public string DisplayInfo { get; protected set; }

        public string DisplayStatus { get; protected set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public DrawLayer Layer { get; protected set; }

        public void Update(float ticks)
        {
            Graphics.Update(ticks);
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            Graphics.CreateResources(createArgs);
        }

        public void Draw(DrawArgs drawArgs)
        {
            Graphics.Draw(drawArgs);
        }

        #endregion
    }
}
