using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Microsoft.Graphics.Canvas.Geometry;

namespace Catch.Mobs
{
    public class BlockMobSharedResources : IGraphicsResource
    {
        public IndicatorCollection Indicators { get; }

        public BlockMobSharedResources()
        {
            // TODO copy down relevant config
            Indicators = new IndicatorCollection();

            int blockSize = 20;

            var strokeStyle = new CanvasStrokeStyle() { LineJoin = CanvasLineJoin.Round };
            var style = new StyleArgs() { BrushType = BrushType.Solid, Color = Colors.Yellow, StrokeWidth = 4, StrokeStyle = strokeStyle };
            Indicators.Add(new BlockMobBaseIndicator(blockSize, style));
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            Indicators.CreateResources(args);
        }

        public void DestroyResources()
        {
            Indicators.DestroyResources();
        }
    }
}
