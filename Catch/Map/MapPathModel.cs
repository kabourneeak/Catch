using System.Collections.Generic;
using Catch.Base;

namespace Catch.Map
{
    public class MapPathModel : IMapPath
    {
        private readonly List<MapTileModel> _tiles;

        public MapPathModel()
        {
            _tiles = new List<MapTileModel>();    
        }

        #region IMapPath Implementation

        public string Name { get; set; }

        public IEnumerable<IMapTile> Tiles => _tiles;

        public int Count => _tiles.Count;

        public int IndexOf(IMapTile tile)
        {
            for (var i = 0; i < Count; ++i)
                if (Equals(_tiles[i].Coords, tile.Coords))
                    return i;

            return -1;
        }

        public IMapTile this[int index] => _tiles[index];

        #endregion

        #region Management

        public void Add(MapTileModel mapTileModel) => _tiles.Add(mapTileModel);

        #endregion
    }
}
