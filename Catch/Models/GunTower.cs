using System;
using System.Numerics;
using Windows.UI;
using Catch.Base;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class GunTower : ITower
    {
        private readonly IConfig _config;

        public GunTower(IConfig config, IHexTile tile)
        {
            _config = config;

            Tile = tile;
            Layer = DrawLayer.Tower;
        }

        #region IGameObject Implementation

        public string DisplayName { get; private set; }

        public string DisplayInfo { get; private set; }

        public string DisplayStatus { get; private set; }

        public Vector2 Position { get { return Tile.Position; } }

        public DrawLayer Layer { get; private set; }

        private float _holdTicks;
        private const float _rotationRate = (float)(2 * Math.PI / 60);
        private float _rotationVel;
        private float _rotation = 0.0f;
        private TileDirection _targetDirection = TileDirection.North;
        private TileDirection _currentDirection = TileDirection.North;

        public void Update(float ticks)
        {
            if (_targetDirection == _currentDirection)
            {
                _holdTicks -= ticks;

                if (_holdTicks < 0)
                {
                    while (_targetDirection == _currentDirection)
                        _targetDirection = TileDirectionExtensions.GetRandom();

                    _rotationVel = _rotationRate *
                                   TileDirectionExtensions.ShortestRotationDirection(_currentDirection, _targetDirection);
                    _holdTicks = 30.0f;
                }
            }
            else
            {
                _rotation = _rotation.Wrap(_rotationVel, 0.0f, (float) (2 * Math.PI));

                if (Math.Abs(_rotation - _targetDirection.CenterRadians()) <= _rotationRate * 1.5f)
                {
                    _currentDirection = _targetDirection;
                    _rotation = _currentDirection.CenterRadians();
                    _rotationVel = 0.0f;
                }
            }
        }

        private static int _createFrameId = -1;
        private static CanvasCachedGeometry _geo;
        private static ICanvasBrush _brush;

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            if (!(createArgs.IsMandatory || _geo == null))
                return;

            if (_createFrameId == createArgs.FrameId)
                return;

            _createFrameId = createArgs.FrameId;

            if (_geo != null)
                _geo.Dispose();

            // define style
            var strokeStyle = new CanvasStrokeStyle() { };
            var strokeWidth = 4;

            // define brush
            _brush = new CanvasSolidColorBrush(createArgs.ResourceCreator, Colors.RoyalBlue);

            // create geometry
            var body = CanvasGeometry.CreateCircle(createArgs.ResourceCreator, new Vector2(0.0f), 24);
            var cannon = CanvasGeometry.CreateRectangle(createArgs.ResourceCreator, -3, 23, 6, 10);
            
            var comb = body.CombineWith(cannon, Matrix3x2.Identity, CanvasGeometryCombine.Union);

            // cache
            _geo = CanvasCachedGeometry.CreateStroke(comb, strokeWidth, strokeStyle);
        }

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.PushRotation(_rotation, Position);
            
            drawArgs.Ds.DrawCachedGeometry(_geo, Position, _brush);

            drawArgs.Pop();
        }

        #endregion

        #region IAgent Implementation

        public string GetAgentType()
        {
            return typeof (GunTower).Name;
        }

        public IBehaviourComponent Brain { get; private set; }

        public IHexTile Tile { get; private set; }

        public bool IsTargetable { get; private set; }

        public int Health { get; private set; }

        public IModifierCollection Modifiers { get; private set; }

        public IIndicatorCollection Indicators { get; private set; }

        public AttackSpecs AttackSpecs { get; private set; }

        public DefenceSpecs DefenceSpecs { get; private set; }

        public IAgentStats Stats { get; private set; }

        #endregion
    }
}
