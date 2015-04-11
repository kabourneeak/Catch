using Windows.UI;
using System.Numerics;
using Catch.Base;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class BlockMob : Mob
    {
        private readonly IConfig _config;

        public BlockMob(MapPath mapPath, IConfig config) : base()
        {
            // This class can be massively generalized!  e.g., take in a config object and a mob name, and then fill out
            // all of the other details like health, speed, attack, defense, mod loadout, from config.

            // TODO copy down relevant config
            _config = config;

            int blockSize = 20;
            Color blockColour = Colors.Yellow;
            float velocity = 0.005f;
            

            Brain = new PathMobBehaviour(this, mapPath, velocity);
            Indicators.Add(new BlockMobBaseIndicator(this, blockSize, blockColour));
        }

        public override string GetAgentType()
        {
            return typeof (BlockMob).Name;
        }
    }

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

    public class BlockMobBaseIndicator : IIndicator
    {
        private readonly Mob _mob;

        private readonly int _blockSize;
        private readonly Color _blockColour;

        public BlockMobBaseIndicator(Mob mob, int blockSize, Color blockColour)
        {
            _mob = mob;
            _blockSize = blockSize;
            _blockColour = blockColour;

            Layer = DrawLayer.Mob;
        }

        public void Update(float ticks)
        {
            // do nothing
        }

        private static int _createFrameId = -1;
        private static CanvasCachedGeometry _geo;
        private static ICanvasBrush _brush;

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            if (!(createArgs.IsMandatory || _geo == null))
                return;

            if (_createFrameId == createArgs.FrameId)
                return;

            _createFrameId = createArgs.FrameId;

            if (_geo != null)
                _geo.Dispose();

            // define style
            var strokeStyle = new CanvasStrokeStyle() { LineJoin = CanvasLineJoin.Round };
            var strokeWidth = 4;

            // define brush
            _brush = new CanvasSolidColorBrush(createArgs.ResourceCreator, _blockColour);

            // create and cache
            var offset = _blockSize / 2.0f;
            var geo = CanvasGeometry.CreateRectangle(createArgs.ResourceCreator, -offset, -offset, _blockSize, _blockSize);

            _geo = CanvasCachedGeometry.CreateStroke(geo, strokeWidth, strokeStyle);
        }

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, _brush);
        }

        public DrawLayer Layer { get; private set; }
    }
}
