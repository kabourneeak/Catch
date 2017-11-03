using System.Numerics;
using Catch.Graphics;

namespace Catch.Base
{
    /// <summary>
    /// Indicators represent components of a graphical object. For example, a Tower may be drawn
    /// as the sum of several Indicators; one for the base image, one for level indicataors, 
    /// another for health, and so on.
    /// 
    /// Indicators should expect to be drawn relative to their parent object. The parent object
    /// is responsible for setting draw transformations appropriately in DrawArgs.
    /// </summary>
    public interface IIndicator
    {
        /// <summary>
        /// Draw this single indicator relative to the current draw arguments
        /// </summary>
        /// <param name="drawArgs">The draw device to use</param>
        void Draw(DrawArgs drawArgs);

        /// <summary>
        /// The world coordinates of the center of the agent
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Only draw this indicator if the requested Layer matches
        /// </summary>
        DrawLayer Layer { get; }

        /// <summary>
        /// Only draw this indicator if the requested LOD is included in this value
        /// </summary>
        DrawLevelOfDetail LevelOfDetail { get; }
    }
}