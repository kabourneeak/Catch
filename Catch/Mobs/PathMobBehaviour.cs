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

        private readonly AgentBase _agent;
        private readonly MapPath _mapPath;
        private PathMobBehaviourStates _state;
        private int _pathIndex;
        private float _velocity;

        public PathMobBehaviour(AgentBase agent, MapPath mapPath, float velocity)
        {
            _agent = agent;
            _mapPath = mapPath;
            _velocity = velocity;
            _state = PathMobBehaviourStates.Advancing;

            _pathIndex = 0;
            _agent.Tile = _mapPath[_pathIndex];
            _agent.TileProgress = 0.5f; // start in the center of our source tile
            _agent.Tile.AddMob(_agent);
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
            _agent.TileProgress += _velocity * e.Ticks;

            // advance to next tile, if necessary
            while (_agent.TileProgress > 1 && _pathIndex < (_mapPath.Count - 1))
            {
                // leave old tile
                _agent.Tile.RemoveMob(_agent);

                // move to next tile
                _pathIndex += 1;
                _agent.TileProgress -= 1.0f;
                _agent.Tile = _mapPath[_pathIndex];
                _agent.Tile.AddMob(_agent);
            }

            // are we done?
            if (_pathIndex == (_mapPath.Count - 1) && _agent.TileProgress >= 0.5f)
                _state = PathMobBehaviourStates.EndOfPath;

            // calculate Position
            UpdatePosition();

            return 1.0f;
        }

        private float UpdateEndOfPath(IUpdateEventArgs e)
        {
            e.Manager.Remove(this._agent);

            return 0.0f;
        }

        private void UpdatePosition()
        {
            Vector2 prev;
            Vector2 next;

            if (_agent.TileProgress < 0.5)
            {
                next = _agent.Tile.Position;
                prev = (_pathIndex > 0) ? _mapPath[_pathIndex - 1].Position : next;
                _agent.Position = Vector2.Lerp(prev, next, 0.5f + _agent.TileProgress);
            }
            else
            {
                prev = _agent.Tile.Position;
                next = (_pathIndex < _mapPath.Count - 1) ? _mapPath[_pathIndex + 1].Position : prev;
                _agent.Position = Vector2.Lerp(prev, next, _agent.TileProgress - 0.5f);
            }
        }

        public void OnRemove()
        {
            _state = PathMobBehaviourStates.Removed;
            _agent.IsActive = false;
        }

        public void OnHit(AttackModel incomingAttack)
        {
            // do nothing
        }
    }
}