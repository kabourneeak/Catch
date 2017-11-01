using Catch.Base;
using Catch.Graphics;
using Catch.Services;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Mobs
{
    public class BlockMobBaseIndicator : IIndicator
    {
        private readonly int _blockSize;
        private readonly IStyle _style;

        public BlockMobBaseIndicator(IConfig config, StyleProvider styleProvider)
        {
            _blockSize = config.GetInt(BlockMobBehaviour.CfgBlockSize);

            _style = styleProvider.GetStyle("BlockMobStyle");

            Layer = DrawLayer.Mob;
        }

        private int _createFrameId = -1;
        private CanvasCachedGeometry _geo;

        public void CreateResources(CreateResourcesArgs args)
        {
            if (!(args.IsMandatory || _geo == null))
                return;

            if (_createFrameId == args.FrameId)
                return;

            DestroyResources();

            _createFrameId = args.FrameId;
            
            // create and cache
            var offset = _blockSize / 2.0f;
            var geo = CanvasGeometry.CreateRectangle(args.ResourceCreator, -offset, -offset, _blockSize, _blockSize);

            _geo = CanvasCachedGeometry.CreateStroke(geo, _style.StrokeWidth, _style.StrokeStyle);
        }

        public void DestroyResources()
        {
            if (_geo == null)
                return;

            _geo.Dispose();
            _geo = null;

            _createFrameId = -1;
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, _style.Brush);
        }

        public DrawLayer Layer { get; }

        public DrawLevelOfDetail LevelOfDetail => DrawLevelOfDetail.NormalHigh;
    }
}