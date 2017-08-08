using System.Collections.Generic;

namespace Catch.Base
{
    /// <summary>
    /// A Map Path, as visible from inside the simulation
    /// </summary>
    public interface IMapPath
    {
        string Name { get; }

        IEnumerable<IMapTile> Tiles { get; }

        int Count { get; }

        int IndexOf(IMapTile tile);

        IMapTile this[int index] { get; }
    }
}
