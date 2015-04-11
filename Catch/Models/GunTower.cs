using System;
using System.Numerics;
using Windows.UI;
using Catch.Base;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class GunTower : BasicTower
    {
        private readonly IConfig _config;

        public GunTower(IHexTile tile, IConfig config) : base(tile)
        {
            _config = config;

            Layer = DrawLayer.Tower;

            Brain = new GunTowerBaseBehaviour(this, config);
            Indicators.Add(new GunTowerBaseIndicator(this, config));
        }

        #region IGameObject Implementation

        // no overrides

        #endregion

        #region IAgent Implementation

        public override string GetAgentType()
        {
            return typeof (GunTower).Name;
        }
        
        #endregion
    }

    public class GunTowerBaseIndicator : IIndicator
    {
        private readonly GunTower _tower;

        public GunTowerBaseIndicator(GunTower tower, IConfig config)
        {
            _tower = tower;
            Layer = DrawLayer.Tower;
        }

        public void Update(float ticks)
        {
            // do nothing
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
            drawArgs.PushRotation(_tower.Rotation);

            drawArgs.Ds.DrawCachedGeometry(_geo, _brush);

            drawArgs.Pop();
        }

        public DrawLayer Layer { get; private set; }
    }

    public class GunTowerBaseBehaviour : IBehaviourComponent
    {
        private readonly GunTower _tower;

        public GunTowerBaseBehaviour(GunTower tower, IConfig config)
        {
            _tower = tower;
        }

        public void OnSpawn()
        {
            // do nothing;
        }

        private float _holdTicks;
        private const float _rotationRate = (float)(2 * Math.PI / 60);
        private float _rotationVel;
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
                _tower.Rotation = _tower.Rotation.Wrap(_rotationVel, 0.0f, (float)(2 * Math.PI));

                if (Math.Abs(_tower.Rotation - _targetDirection.CenterRadians()) <= _rotationRate * 1.5f)
                {
                    _currentDirection = _targetDirection;
                    _tower.Rotation = _currentDirection.CenterRadians();
                    _rotationVel = 0.0f;
                }
            }
        }

        public void OnRemove()
        {
            // do nothing
        }

        public void OnAttacked(IAttack attack)
        {
            // do nothing
        }
    }
}
