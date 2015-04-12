using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Models
{
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