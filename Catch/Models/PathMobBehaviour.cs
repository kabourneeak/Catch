using System;
using System.Numerics;
using Catch.Base;

namespace Catch.Models
{
    public class PathMobBehaviour : IBehaviourComponent
    {
        private enum PathMobBehaviourStates
        {
            Advancing, EndOfPath, Removed
        }

        private readonly Mob _mob;
        private readonly MapPath _mapPath;
        private PathMobBehaviourStates _state;
        private int _pathIndex;
        private float _velocity;

        public PathMobBehaviour(Mob mob, MapPath mapPath, float velocity)
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

        public void Update(float ticks)
        {
            switch (_state)
            {
                case PathMobBehaviourStates.Advancing:
                    UpdateAdvancing(ticks);
                    break;
                case PathMobBehaviourStates.EndOfPath:
                    UpdateEndOfPath(ticks);
                    break;
                case PathMobBehaviourStates.Removed:
                    // do nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateAdvancing(float ticks)
        {

            // advance through tile
            _mob.TileProgress += _velocity * ticks;

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
        }

        private void UpdateEndOfPath(float ticks)
        {
            OnRemove();
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
            _mob.Tile.RemoveMob(_mob);
        }

        public void OnAttacked(IAttack attack)
        {
            // do nothing
        }
    }
}