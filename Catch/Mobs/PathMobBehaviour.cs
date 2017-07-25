using System;
using System.Numerics;
using Catch.Base;
using Catch.Map;

namespace Catch.Mobs
{
    public class PathMobBehaviour : IBehaviourComponent
    {
        private enum PathMobBehaviourStates
        {
            Advancing, EndOfPath, Removed
        }

        private readonly AgentBase _mob;
        private readonly MapPath _mapPath;
        private PathMobBehaviourStates _state;
        private int _pathIndex;
        private float _velocity;

        public PathMobBehaviour(AgentBase mob, MapPath mapPath, float velocity)
        {
            _mob = mob;
            _mapPath = mapPath;
            _velocity = velocity;
            _state = PathMobBehaviourStates.Advancing;

            _pathIndex = 0;
            _mob.Tile = _mapPath[_pathIndex];
            _mob.TileProgress = 0.5f; // start in the center of our source tile
            _mob.Tile.AddMob(_mob);
        }

        public float Update(IUpdateEventArgs e)
        {
            switch (_state)
            {
                case PathMobBehaviourStates.Advancing:
                    return UpdateAdvancing(e);
                case PathMobBehaviourStates.EndOfPath:
                    return UpdateEndOfPath(e);
                case PathMobBehaviourStates.Removed:
                    // do nothing
                    return 0.0f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float UpdateAdvancing(IUpdateEventArgs e)
        {
            // advance through tile
            _mob.TileProgress += _velocity * e.Ticks;

            // advance to next tile, if necessary
            while (_mob.TileProgress > 1 && _pathIndex < (_mapPath.Count - 1))
            {
                // leave old tile
                _mob.Tile.RemoveMob(_mob);

                // move to next tile
                _pathIndex += 1;
                _mob.TileProgress -= 1.0f;
                _mob.Tile = _mapPath[_pathIndex];
                _mob.Tile.AddMob(_mob);
            }

            // are we done?
            if (_pathIndex == (_mapPath.Count - 1) && _mob.TileProgress >= 0.5f)
                _state = PathMobBehaviourStates.EndOfPath;

            // calculate Position
            UpdatePosition();

            return 1.0f;
        }

        private float UpdateEndOfPath(IUpdateEventArgs e)
        {
            OnRemove();

            return 0.0f;
        }

        private void UpdatePosition()
        {
            Vector2 prev;
            Vector2 next;

            if (_mob.TileProgress < 0.5)
            {
                next = _mob.Tile.Position;
                prev = (_pathIndex > 0) ? _mapPath[_pathIndex - 1].Position : next;
                _mob.Position = Vector2.Lerp(prev, next, 0.5f + _mob.TileProgress);
            }
            else
            {
                prev = _mob.Tile.Position;
                next = (_pathIndex < _mapPath.Count - 1) ? _mapPath[_pathIndex + 1].Position : prev;
                _mob.Position = Vector2.Lerp(prev, next, _mob.TileProgress - 0.5f);
            }
        }

        public void OnRemove()
        {
            _state = PathMobBehaviourStates.Removed;
            _mob.IsActive = false;
        }

        public void OnHit(AttackModel incomingAttack)
        {
            // do nothing
        }
    }
}