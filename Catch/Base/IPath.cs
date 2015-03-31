using System.Collections.Generic;

namespace Catch.Base
{
    /// <summary>
    /// A named list of tiles that objects can follow over time.
    /// </summary>
    public interface IPath : IList<IHexTile>
    {
        string Name { get; }
    }
}
