using System.Numerics;
using Catch.Graphics;
using Microsoft.Graphics.Canvas;

namespace Catch.Level
{
    public class PrerenderedLayer : IGraphicsResource
    {
        public DrawLevelOfDetail LevelOfDetail { get; }

        public DrawLayer Layer { get; }

        public int Version { get; set; }

        public CanvasBitmap Render { get; set; }

        public Vector2 Offset { get; set; }

        public bool IsCreated => Render != null;

        public PrerenderedLayer(DrawLevelOfDetail levelOfDetail, DrawLayer layer)
        {
            LevelOfDetail = levelOfDetail;
            Layer = layer;
            Offset = Vector2.Zero;
        }

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            // do nothing
        }

        public void DestroyResources()
        {
            if (Render != null)
            {
                Render.Dispose();
                Render = null;
            }
        }
    }
}
