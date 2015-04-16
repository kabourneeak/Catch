using System;
using Catch.Base;
using Catch.Services;
using System.Numerics;

namespace Catch.Models
{
    public class GunTowerBaseBehaviour : IBehaviourComponent
    {
        private TowerBehaviourState _state;
        private readonly GunTower _tower;
        private readonly Targetting _targetting;

        public GunTowerBaseBehaviour(GunTower tower, IConfig config)
        {
            _tower = tower;
            _targetting = new RadiusExitTargetting(tower.Tile, 1, 1);
            _state = TowerBehaviourState.Targetting;
        }

        public void OnSpawn()
        {
            // do nothing;
        }

        private float _holdTicks;
        private const float _rotationRate = (float)(2 * Math.PI / 60);
        private float _rotationVel;
        private float _targetDirection = 0.0f;
        private float _currentDirection = 0.0f;

        public void Update(float ticks)
        {
            switch (_state)
            {
                case TowerBehaviourState.Targetting:
                    UpdateTargetting(ticks);
                    break;
                case TowerBehaviourState.Aiming:
                    UpdateAiming(ticks);
                    break;
                case TowerBehaviourState.OnTarget:
                    UpdateOnTarget(ticks);
                    break;
                case TowerBehaviourState.TargetLost:
                    UpdateTargetLost(ticks);
                    break;
                case TowerBehaviourState.Removed:
                    // do nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Mob _targetMob;
        private Tile _targetTile;

        private void UpdateTargetting(float ticks)
        {
            _targetMob = _targetting.GetBestTargetMob();

            if (_targetMob != null)
            {
                _targetTile = _targetMob.Tile;

                var a = _tower.Position;
                var b = _targetMob.Position;
                _targetDirection = (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
                _rotationVel = _rotationRate * Targetting.ShortestRotationDirection(_currentDirection, _targetDirection);

                _state = TowerBehaviourState.Aiming;
            }
        }

        private void UpdateAiming(float ticks)
        {
            const float twopi = (float) Math.PI * 2;

            // find rotation angle from us to target tile
            var a = _tower.Position;
            var b = _targetMob.Position;
            _targetDirection = (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
            _currentDirection = _currentDirection.Wrap(_rotationVel, 0.0f, twopi);

            // see if we've arrived
            if (Math.Abs(_currentDirection.Wrap(-_targetDirection, 0.0f, twopi)) <= _rotationRate * 1.5f)
            {
                _currentDirection = _targetDirection;
                _state = TowerBehaviourState.OnTarget;
            }

            _tower.Rotation = _currentDirection;
        }

        private void UpdateOnTarget(float ticks)
        {
            // check if mob has become untargetable
            if (_targetMob.IsTargetable == false)
            {
                _state = TowerBehaviourState.TargetLost;
                return;
            }

            // find rotation angle from us to target tile
            const float twopi = (float)Math.PI * 2;
            var a = _tower.Position;
            var b = _targetMob.Position;
            _targetDirection = (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
            _currentDirection = _targetDirection;

            _tower.Rotation = _currentDirection;

            // check if mob is out of range
            if (_targetMob.Tile != _targetTile)
            {
                _state = TowerBehaviourState.Targetting;
            }

            // fire
        }

        private void UpdateTargetLost(float ticks)
        {
            _targetMob = _targetting.GetBestTargetMob(_targetTile);

            if (_targetMob == null)
            {
                // no other targets in this tile, look for a new tile
                _state = TowerBehaviourState.Targetting;
            }
            else
            {
                // switch to the next target in this tile
                _state = TowerBehaviourState.Aiming;
            }
        }

        public void OnRemove()
        {
            _state = TowerBehaviourState.Removed;
        }

        public void OnAttacked(IAttack attack)
        {
            // do nothing
        }
    }

    public enum TowerBehaviourState
    {
        Targetting, Aiming, OnTarget, TargetLost, Removed
    }
}