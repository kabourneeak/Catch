using System.Numerics;
using Catch.Base;

namespace Catch.Graphics
{
    /// <summary>
    /// An object which can be drawn
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// The world coordinates of the center of the agent
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// The logical rotation of the object. Each indicator will decide for itself whether it 
        /// will use this information (e.g., a tower body might be rotated, but its health bar 
        /// stays at the top)
        /// </summary>
        float Rotation { get; }

        /// <summary>
        /// The graphical indications that should be drawn for this object
        /// </summary>
        IndicatorCollection Indicators { get; }
    }
}