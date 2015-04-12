using System.Numerics;
using Catch.Base;

namespace Catch.Models
{
    public class PathMobBehaviour : IBehaviourComponent
    {
        private readonly Mob _mob;
        private readonly MapPath _mapPath;
        private int _pathIndex;
        private float _tileProgress;
        private float _velocity;

        public PathMobBehaviour(Mob mob, MapPath mapPath, float velocity)
        {
            _mob = mob;
            _mapPath = mapPath;
            _velocity = velocity;

            _pathIndex = 0;
            _mob.Tile = _mapPath[_pathIndex];
            _tileProgress = 0.5f; // start in the center of our source tile
        }

        public void OnSpawn()
        {
            // do nothing
        }

        public void Update(float ticks)
        {
            // advance through tile
            _tileProgress += _velocity * ticks;

            // advance to next tile, if necessary
            while (_tileProgress > 1 && _pathIndex < (_mapPath.Count - 1))
            {
                _pathIndex += 1;
                _tileProgress -= 1.0f;
                _mob.Tile = _mapPath[_pathIndex];
            }

            // calculate Position
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Vector2 prev;
            Vector2 next;

            if (_tileProgress < 0.5)
            {
                next = _mob.Tile.Position;
                prev = (_pathIndex > 0) ? _mapPath[_pathIndex - 1].Position : next;
                _mob.Position = Vector2.Lerp(prev, next, 0.5f + _tileProgress);
            }
            else
            {
                prev = _mob.Tile.Position;
                next = (_pathIndex < _mapPath.Count - 1) ? _mapPath[_pathIndex + 1].Position : prev;
                _mob.Position = Vector2.Lerp(prev, next, _tileProgress - 0.5f);
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