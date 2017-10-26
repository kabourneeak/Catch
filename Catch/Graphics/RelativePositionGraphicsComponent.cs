namespace Catch.Graphics
{
    /// <summary>
    /// Draws an <see cref="IDrawable"/> by first translating the draw args
    /// to the position of the drawable. Each indicator should then draw itself 
    /// relative to the origin
    /// </summary>
    public class RelativePositionGraphicsComponent : IGraphicsComponent
    {
        public RelativePositionGraphicsComponent()
        {

        }

        public void Draw(IDrawable drawable, DrawArgs drawArgs)
        {
            if (drawable.Indicators.Count == 0)
                return;

            // translate relative to the drawable's position
            drawArgs.PushTranslation(drawable.Position);

            // draw request layers and levels of detail
            foreach (var i in drawable.Indicators)
                if (i.Layer == drawArgs.Layer && i.LevelOfDetail.HasFlag(drawArgs.LevelOfDetail))
                    i.Draw(drawArgs, drawable.Rotation);

            drawArgs.Pop();
        }
    }
}
