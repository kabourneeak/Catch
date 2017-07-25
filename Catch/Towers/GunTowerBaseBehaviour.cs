using System;
using Catch.Base;
using Catch.Map;
using Catch.Mobs;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerBaseBehaviour : IBehaviourComponent
    {
        private TowerBehaviourState _state;
        private readonly GunTower _tower;
        private readonly TargettingBase _targetting;

        public GunTowerBaseBehaviour(GunTower tower, IConfig config)
        {
            _tower = tower;
            _targetting = new RadiusExitTargetting(tower.Tile, 1, 1);
            _state = TowerBehaviourState.Targetting;
        }

        private const float RotationRate = (float)(2 * Math.PI / 60);
        private const float Twopi = (float) Math.PI * 2;

        private float _rotationVel;
        private float _targetDirection = 0.0f;
        private float _currentDirection = 0.0f;

        public float Update(IUpdateEventArgs e)
        {
            switch (_state)
            {
                case TowerBehaviourState.Targetting:
                    UpdateTargetting(e.Ticks);
                    break;
                case TowerBehaviourState.Aiming:
                    UpdateAiming(e.Ticks);
                    break;
                case TowerBehaviourState.OnTarget:
                    UpdateOnTarget(e.Ticks);
                    break;
                case TowerBehaviourState.Removed:
                    // do nothing
                    return 0.0f;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return 1.0f;
        }

        private IAgent _targetMob;
        private Tile _targetTile;

        private void UpdateTargetting(float ticks)
        {
            _targetMob = _targetting.GetBestTargetMob();

            if (_targetMob != null)
            {
                _targetTile = _targetMob.Tile;

                // determine rotation speed and direction
                CalcTargetDirection();
                _rotationVel = RotationRate * TargettingBase.ShortestRotationDirection(_currentDirection, _targetDirection);

                _state = TowerBehaviourState.Aiming;
            }
        }

        private void UpdateAiming(float ticks)
        {
            // find rotation angle from us to target tile
            CalcTargetDirection();
            _currentDirection = _currentDirection.Wrap(_rotationVel * ticks, 0.0f, Twopi);

            // see if we've arrived
            if (Math.Abs(_currentDirection.Wrap(-_targetDirection, 0.0f, Twopi)) <= RotationRate * ticks * 1.5f)
            {
                _currentDirection = _targetDirection;
                _state = TowerBehaviourState.OnTarget;
            }

            _tower.Rotation = _currentDirection;
        }

        private void UpdateOnTarget(float ticks)
        {
            // check if mob has become untargetable
            if (!_targetMob.IsActive || _targetMob.Tile != _targetTile)
            {
                _state = TowerBehaviourState.Targetting;
                return;
            }
            
            // follow the target
            CalcTargetDirection();
            _currentDirection = _targetDirection;
            _tower.Rotation = _currentDirection;

            // TODO fire at enemy
        }

        private void CalcTargetDirection()
        {
            var a = _tower.Position;
            var b = _targetMob.Position;
            _targetDirection = (float) Math.Atan2(b.Y - a.Y, b.X - a.X);
        }

        public void OnRemove()
        {
            _state = TowerBehaviourState.Removed;
            _tower.IsActive = false;
        }

        public void OnHit(AttackModel incomingAttack)
        {
            // do nothing
        }
    }

    public enum TowerBehaviourState
    {
        Targetting, Aiming, OnTarget, Removed
    }
}