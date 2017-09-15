using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerBaseBehaviour : IBehaviourComponent
    {
        private const float DefaultGameSpeed = 60.0f;
        private readonly GunTower _tower;
        private TowerBehaviourState _state;
        private TargettingBase _targetting;

        public GunTowerBaseBehaviour(GunTower tower, IConfig config)
        {
            _tower = tower;
            _state = TowerBehaviourState.Init;

            // TODO get default game speed from config and use for scheduling
        }

        private const float RotationRate = (float)(2 * Math.PI / 60);
        private const float Twopi = (float) Math.PI * 2;

        private float _rotationVel;
        private float _currentDirection = 0.0f;

        public float Update(IUpdateEventArgs args)
        {
            switch (_state)
            {
                case TowerBehaviourState.Init:
                    UpdateInit(args);
                    break;
                case TowerBehaviourState.Searching:
                    // TODO search less often
                    UpdateSearching();
                    break;
                case TowerBehaviourState.Aiming:
                    UpdateAiming(args.Ticks);
                    break;
                case TowerBehaviourState.OnTarget:
                    UpdateOnTarget();
                    break;
                case TowerBehaviourState.Removed:
                    // do nothing
                    return 0.0f;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return DefaultGameSpeed / 60;
        }

        private IAgent _targetMob;
        private IMapTile _targetTile;

        private void UpdateInit(IUpdateEventArgs args)
        {
            _targetting = new RadiusExitTargetting(args.Sim.Map, _tower.Tile, 1, 1);
            _state = TowerBehaviourState.Searching;
        }

        private void UpdateSearching()
        {
            // see if anything has changed in our targetting range
            if (_targetting.GetAgentVersionDelta() == 0)
                return;

            // if so, select a target
            _targetTile = _targetting.GetBestTargetTile();
            _targetMob = _targetting.GetBestTargetMob(_targetTile);

            if (_targetMob == null)
            {
                // no target found, continue searching    
            }
            else
            {
                // target found; start aiming
                _state = TowerBehaviourState.Aiming;
            }
        }

        private void UpdateAiming(float ticks)
        {
            // find rotation angle from us to target
            var targetDirection = CalcTargetDirection();

            // update our direction according to our rotation rate
            _rotationVel = RotationRate * TargettingBase.ShortestRotationDirection(_currentDirection, targetDirection);
            _currentDirection = _currentDirection.Wrap(_rotationVel * ticks, 0.0f, Twopi);

            // see if we've arrived nearly enough, and snap
            if (Math.Abs(_currentDirection.Wrap(-targetDirection, 0.0f, Twopi)) <= RotationRate * ticks)
            {
                _currentDirection = targetDirection;
                _state = TowerBehaviourState.OnTarget;
            }

            _tower.Rotation = _currentDirection;
        }

        private void UpdateOnTarget()
        {
            // check if a better target has appeared
            if (_targetting.GetAgentVersionDelta() > 0)
            {
                var potentialTile = _targetting.GetBestTargetTile();
                var potentialMob = _targetting.GetBestTargetMob(potentialTile);

                if (ReferenceEquals(potentialMob, _targetMob))
                {
                    // stay on target
                }
                else if (potentialMob == null)
                {
                    // no targets available
                    _state = TowerBehaviourState.Searching;
                    return;
                }
                else
                {
                    // a better target has appeared
                    _targetTile = potentialTile;
                    _targetMob = potentialMob;

                    _state = TowerBehaviourState.Aiming;
                    return;
                }
            }
            
            // continue to point at the target
            _currentDirection = CalcTargetDirection();
            _tower.Rotation = _currentDirection;

            // TODO fire at enemy
        }

        private float CalcTargetDirection()
        {
            var a = _tower.Position;
            var b = _targetMob.Position;
            return (float) Math.Atan2(b.Y - a.Y, b.X - a.X);
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
        Init, Searching, Aiming, OnTarget, Removed
    }
}