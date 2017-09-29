using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Towers
{
    public class GunTower : TowerBase, IAgentStatsModifier, IRemoveModifier
    {
        public GunTower(GunTowerSharedResources resources, IMapTile tile) : base(nameof(GunTower), tile)
        {
            // initialize properties
            DisplayName = "Gun Tower";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;

            AddModifier(this);

            Indicators.AddRange(resources.Indicators);

            // initialize behaviour

            _ticksPerSecond = resources.Config.GetFloat(CoreConfig.TicksPerSecond);

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

        public override float Update(IUpdateEventArgs args)
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
            _targetting = new RadiusExitTargetting(args.Sim.Map, Tile, 1, 1);
            _targetting.OwnTeam = Stats.Team;

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

            Rotation = _currentDirection;
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
            Rotation = _currentDirection;

            // TODO fire at enemy
            var attack = this.CreateAttack(_targetMob);
            _targetMob.OnHit(attack);
        }

        private float CalcTargetDirection()
        {
            var a = this.Position;
            var b = _targetMob.Position;
            return (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
        }

        #endregion

        #region Modifier implementations

        public ModifierPriority Priority => ModifierPriority.Base;

        public void OnCalculateAgentStats(IExtendedAgent agent)
        {
            agent.ExtendedStats.MaxHealth = 100;
            agent.ExtendedStats.Health = 100;
            agent.ExtendedStats.Level = 1;
        }

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
