﻿using System.Numerics;
using Catch.Base;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public abstract class PathMobAgent : IPathAgent
    {
        protected PathMobAgent(IPath path, IBehaviourComponent brain)
        {
            Path = path;
            Brain = brain;

            InitMobOnPath();
        }

        private void InitMobOnPath()
        {
            Velocity = (1 / 60.0f);
            PathIndex = 0;
            Tile = Path[PathIndex];
            TileProgress = 0.5f; // start in the center of our source tile
        }

        #region IPathAgent Implementation

        public float TileProgress { get; protected set; }

        public float Velocity { get; set; }

        public IPath Path { get; protected set; }

        public int PathIndex { get; set; }

        #endregion

        #region IAgent Implementation

        public abstract string GetAgentType();

        public IBehaviourComponent Brain { get; protected set; }

        public IHexTile Tile { get; protected set; }

        public bool IsTargetable { get; protected set; }

        public int Health { get; protected set; }

        public IModifierCollection Modifiers { get; protected set; }

        public IIndicatorCollection Indicators { get; protected set; }

        public AttackSpecs AttackSpecs { get; protected set; }

        public DefenceSpecs DefenceSpecs { get; protected set; }

        public IAgentStats Stats { get; protected set; }

        #endregion

        #region IGameObject implementation

        public string DisplayName { get; protected set; }

        public string DisplayInfo { get; protected set; }

        public string DisplayStatus { get; protected set; }

        public Vector2 Position { get; protected set; }

        public DrawLayer Layer { get; protected set; }

        public virtual void Update(float ticks)
        {
            // advance through tile
            TileProgress += Velocity * ticks;

            // advance to next tile, if necessary
            while (TileProgress > 1 && PathIndex < (Path.Count - 1))
            {
                PathIndex += 1;
                TileProgress -= 1.0f;
                Tile = Path[PathIndex];
            }

            // calculate Position
            UpdatePosition();

            Brain.Update(ticks);
        }

        private void UpdatePosition()
        {
            Vector2 prev;
            Vector2 next;

            if (TileProgress < 0.5)
            {
                next = Tile.Position;
                prev = (PathIndex > 0) ? Path[PathIndex - 1].Position : next;
                Position = Vector2.Lerp(prev, next, 0.5f + TileProgress);
            }
            else
            {
                prev = Tile.Position;
                next = (PathIndex < Path.Count - 1) ? Path[PathIndex + 1].Position : prev;
                Position = Vector2.Lerp(prev, next, TileProgress - 0.5f);
            }
        }

        public virtual void CreateResources(DrawArgs drawArgs)
        {
            // do nothing
        }

        public virtual void Draw(DrawArgs drawArgs)
        {
            // do nothing
        }

        #endregion
    }
}