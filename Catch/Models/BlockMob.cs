using System.Linq.Expressions;
using Windows.Foundation;
using Windows.UI;
using Catch.Base;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class BlockMob : PathMobAgent
    {
        private readonly IConfig _config;
        private readonly int _blockSize;
        private readonly Color _blockColour;

        public BlockMob(IPath path, IBehaviourComponent brain, IConfig config) : base(path, brain)
        {
            // This class can be massively generalized!  e.g., take in a config object and a mob name, and then fill out
            // all of the other details like health, speed, attack, defense, mod loadout, from config.

            _config = config;

            Velocity = 0.005f;

            // TODO copy down relevant config
            _blockSize = 20;
            _blockColour = Colors.Yellow;
        }

        public override string GetAgentType()
        {
            return typeof (BlockMob).Name;
        }

        private static int _createFrameId = -1;
        private static CanvasCachedGeometry _geo;
        private static ICanvasBrush _brush;

        public override void CreateResources(CreateResourcesArgs createArgs)
        {
            if (!(createArgs.IsMandatory || _geo == null))
                return;

            if (_createFrameId == createArgs.FrameId)
                return;

            _createFrameId = createArgs.FrameId;

            if (_geo != null)
                _geo.Dispose();

            // define style
            var strokeStyle = new CanvasStrokeStyle() {LineJoin = CanvasLineJoin.Round};
            var strokeWidth = 4;

            // define brush
            _brush = new CanvasSolidColorBrush(createArgs.ResourceCreator, _blockColour);

            // create and cache
            var offset = _blockSize / 2.0f;
            var geo = CanvasGeometry.CreateRectangle(createArgs.ResourceCreator, -offset, -offset, _blockSize, _blockSize);

            _geo = CanvasCachedGeometry.CreateStroke(geo, strokeWidth, strokeStyle);
        }

        public override void Draw(DrawArgs drawArgs)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, Position, _brush);
        }
    }
}
