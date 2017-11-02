using System;
using Catch.Base;
using Catch.Components;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTowerBehaviour : IUpdatable, IRemoveModifier
    {
        public const string AgentTypeName = "GunTower";

        private readonly IExtendedAgent _host;

        public GunTowerBehaviour(IExtendedAgent host, IConfig config)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            if (config == null) throw new ArgumentNullException(nameof(config));

            _host.Position = _host.Tile.Position;

            // initialize behaviour
            _ticksPerSecond = config.GetFloat(CoreConfig.TicksPerSecond);

            _state = TowerBehaviourState.Init;
        }

        #region IUpdatable

        private const float RotationRate = (float)(2 * Math.PI / 60);
        private const float Twopi = (float)Math.PI * 2;

        private const int InitResolution = 60;
        private const int SearchResolution = 5;
        private const int AimingResolution = 60;
        private const int AttackResolution = 60;

        private readonly float _ticksPerSecond;
        private TowerBehaviourState _state;
        private TargettingBase _targetting;
        private float _rotationVel;
        private float _currentDirection = 0.0f;
        private IAgent _targetMob;
        private IMapTile _targetTile;

        public float Update(IUpdateEventArgs args)
        {
            // Update according to the current state of the agent
            switch (_state)
            {
                case TowerBehaviourState.Init:
                    UpdateInit(args);
                    break;
                case TowerBehaviourState.Searching:
                    UpdateSearching();
                    break;
                case TowerBehaviourState.Aiming:
                    UpdateAiming(args.Ticks);
                    break;
                case TowerBehaviourState.Attacking:
                    UpdateAttacking();
                    break;
                case TowerBehaviourState.Removed:
                    return 0.0f;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Reschedule according to the (potentially updated) state of the agent
            switch (_state)
            {
                case TowerBehaviourState.Init:
                    return _ticksPerSecond / InitResolution;
                case TowerBehaviourState.Searching:
                    return _ticksPerSecond / SearchResolution;
                case TowerBehaviourState.Aiming:
                    return _ticksPerSecond / AimingResolution;
                case TowerBehaviourState.Attacking:
                    return _ticksPerSecond / AttackResolution;
                case TowerBehaviourState.Removed:
                    return 0.0f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateInit(IUpdateEventArgs args)
        {
            _targetting = new RadiusExitTargetting(args.Sim.Map, _host.Tile, 1, 1);
            _targetting.OwnTeam = _host.Stats.Team;

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
                _state = TowerBehaviourState.Attacking;
            }

            _host.Rotation = _currentDirection;
        }

        private void UpdateAttacking()
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
            _host.Rotation = _currentDirection;

            // TODO fire at enemy
            var attack = _host.CreateAttack(_targetMob);
            _targetMob.OnHit(attack);
        }

        private float CalcTargetDirection()
        {
            var a = _host.Position;
            var b = _targetMob.Position;
            return (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
        }

        #endregion

        #region Modifier implementations

        public ModifierPriority Priority => ModifierPriority.Base;

        public void OnRemove(IExtendedAgent agent)
        {
            _state = TowerBehaviourState.Removed;
        }

        #endregion

        private enum TowerBehaviourState
        {
            Init, Searching, Aiming, Attacking, Removed
        }

    }
}
