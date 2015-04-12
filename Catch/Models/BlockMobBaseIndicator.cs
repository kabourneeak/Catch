using Windows.UI;
using Catch.Base;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
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

        public DrawLayer Layer { get; set; }
    }
}