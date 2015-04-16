using Catch.Base;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class BlockMobBaseIndicator : IIndicator
    {
        private readonly int _blockSize;
        private readonly StyleArgs _style;

        public BlockMobBaseIndicator(int blockSize, StyleArgs styleArgs)
        {
            _blockSize = blockSize;
            _style = styleArgs;

            Layer = DrawLayer.Mob;
        }

        public void Update(float ticks)
        {
            // do nothing
        }

        private int _createFrameId = -1;
        private CanvasCachedGeometry _geo;
        private ICanvasBrush _brush;

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            if (!(createArgs.IsMandatory || _geo == null))
                return;

            if (_createFrameId == createArgs.FrameId)
                return;

            _createFrameId = createArgs.FrameId;

            if (_geo != null)
                _geo.Dispose();

            // define brush
            _brush = _style.CreateBrush(createArgs);

            // create and cache
            var offset = _blockSize / 2.0f;
            var geo = CanvasGeometry.CreateRectangle(createArgs.ResourceCreator, -offset, -offset, _blockSize, _blockSize);

            _geo = CanvasCachedGeometry.CreateStroke(geo, _style.StrokeWidth, _style.StrokeStyle);
        }

        public void Draw(DrawArgs drawArgs)
        {
            drawArgs.Ds.DrawCachedGeometry(_geo, _brush);
        }

        public DrawLayer Layer { get; set; }
    }
}