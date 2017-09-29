using System;
using System.Numerics;
using Catch.Base;

namespace Catch.Mobs
{
    public class PathMobBehaviour : IBehaviourComponent
    {
        private enum PathMobBehaviourStates
        {
            Init, Advancing, EndOfPath, Removed
        }

        private readonly AgentBase _agent;
        private readonly IMapPath _mapPath;
        private PathMobBehaviourStates _state;
        private int _pathIndex;
        private float _velocity;

        public PathMobBehaviour(AgentBase agent, IMapPath mapPath, float velocity)
        {
            _agent = agent;
            _mapPath = mapPath;
            _velocity = velocity;
            _state = PathMobBehaviourStates.Init;
        }

        public float Update(IUpdateEventArgs args)
        {
            switch (_state)
            {
                case PathMobBehaviourStates.Init:
                    return UpdateInit(args);
                case PathMobBehaviourStates.Advancing:
                    return UpdateAdvancing(args);
                case PathMobBehaviourStates.EndOfPath:
                    return UpdateEndOfPath(args);
                case PathMobBehaviourStates.Removed:
                    // do nothing
                    return 0.0f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float UpdateInit(IUpdateEventArgs args)
        {
            // move into initial tile
            _pathIndex = 0;
            _agent.Tile = args.Manager.Move(_agent, _mapPath[_pathIndex]);
            _agent.TileProgress = 0.5f; // start in the center of our source tile

            _state = PathMobBehaviourStates.Advancing;

            return 1.0f;
        }

        private float UpdateAdvancing(IUpdateEventArgs args)
        {
            // advance through tile
            _agent.TileProgress += _velocity * args.Ticks;

            // advance to next tile, if necessary
            while (_agent.TileProgress > 1 && _pathIndex < (_mapPath.Count - 1))
            {
                // move to next tile
                _pathIndex += 1;
                _agent.TileProgress -= 1.0f;
                _agent.Tile = args.Manager.Move(_agent, _mapPath[_pathIndex]);
            }

            // are we done?
            if (_pathIndex == (_mapPath.Count - 1) && _agent.TileProgress >= 0.5f)
                _state = PathMobBehaviourStates.EndOfPath;

            // calculate Position
            UpdatePosition();

            return 1.0f;
        }

        private float UpdateEndOfPath(IUpdateEventArgs args)
        {
            args.Manager.Remove(this._agent);

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
        }
    }
}