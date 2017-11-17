using System;
using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;
using CatchLibrary.HexGrid;
using Microsoft.Graphics.Canvas;

namespace Catch.Level
{
    public class PrerenderProvider : IProvider, IGraphicsResourceContainer
    {
        private static readonly DrawLayer[] DrawLayers = EnumUtils.GetEnumAsList<DrawLayer>().ToArray();

        private readonly IIndicatorRegistry _indicatorRegistry;
        private readonly Dictionary<DrawLayer, PrerenderedLayer> _prerenders;

        private readonly float _tileRadius;
        private readonly float _tileRadiusH;
        
        public PrerenderProvider(IConfig config, IIndicatorRegistry indicatorRegistry)
        {
            _indicatorRegistry = indicatorRegistry ?? throw new ArgumentNullException(nameof(indicatorRegistry));

            _tileRadius = config.GetFloat(CoreConfig.TileRadius);
            _tileRadiusH = HexUtils.GetRadiusHeight(_tileRadius);

            _prerenders = new Dictionary<DrawLayer, PrerenderedLayer>();

            foreach (var dl in DrawLayers)
                _prerenders.Add(dl, new PrerenderedLayer(DrawLevelOfDetail.Low, dl));
        }

        /// <summary>
        /// Gets the pre-render for the given lod and layer, or null if no pre-render is 
        /// available.
        /// </summary>
        public CanvasBitmap GetPrerender(DrawLevelOfDetail lod, DrawLayer layer)
        {
            if (lod != DrawLevelOfDetail.Low)
                return null;

            return _prerenders[layer].Render;
        }

        public void CreatePrerenders(DrawArgs drawArgs, Vector2 size)
        {
            var prerender = _prerenders[DrawLayer.Background];

            if (prerender.Version == _indicatorRegistry.GetVersion(prerender.LevelOfDetail, prerender.Layer))
            {
                // prerender is still up to date    
            }
            else
            {
                CreatePrerender(prerender, drawArgs, size);
            }
        }

        private void CreatePrerender(PrerenderedLayer prerender, DrawArgs drawArgs, Vector2 size)
        {
            prerender.DestroyResources();

            var indicators = _indicatorRegistry.GetIndicators(prerender.LevelOfDetail, prerender.Layer);
            prerender.Version = _indicatorRegistry.GetVersion(prerender.LevelOfDetail, prerender.Layer);

            CanvasRenderTarget offscreen = new CanvasRenderTarget(drawArgs.Ds.Device, size.X + 2 * _tileRadius, size.Y + 4 * _tileRadiusH, drawArgs.Ds.Dpi);
            using (CanvasDrawingSession ds = offscreen.CreateDrawingSession())
            {
                var prerenderArgs = new DrawArgs(ds, drawArgs.CurrentTransform, drawArgs.FrameId);

                prerenderArgs.PushTranslation(_tileRadius, 2.0f * _tileRadiusH);

                foreach (var indicator in indicators)
                    indicator.Draw(prerenderArgs);
            }

            prerender.Render = offscreen;
        }

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            // do nothing
        }

        public void DestroyResources()
        {
            foreach (var prerender in _prerenders.Values)
                prerender.DestroyResources();
        }
    }
}
