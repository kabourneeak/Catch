using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Catch.Services;
using CatchLibrary;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    /// <summary>
    /// Implements a "living" map or field of play, which has tiles, towers, 
    /// paths.
    /// </summary>
    public class Map : IEnumerable<Tile>
    {
        private readonly IConfig _config;
        private HexGridCollection<Tile> _tiles; 
        private Dictionary<string, MapPath> _paths;
        private readonly float _tileRadius;

        public Map(IConfig config)
        {
            _config = config;

            _tileRadius = config.GetFloat("TileRadius");
        }

        public void Initialize(int rows, int columns)
        {
            DebugUtils.Assert(rows >= 1);
            DebugUtils.Assert(columns >= 1);

            Rows = rows;
            Columns = columns;
            Size = new Vector2((float)(Columns * _tileRadius * 1.5 + _tileRadius / 2), (float)(Rows * 2 * HexUtils.GetRadiusHeight(_tileRadius)));

            _tiles = new HexGridCollection<Tile>(Rows, Columns);
            _tiles.Populate((r, c, v) => new Tile(r, c, this, _config));

            _paths = new Dictionary<string, MapPath>();
        }

        #region Hexagonal Grid

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public Vector2 Size { get; private set; }

        public Tile GetTile(int row, int col) => _tiles.GetHex(row, col);

        public Tile GetNeighbour(Tile tile, TileDirection direction) => _tiles.GetNeighbour(tile.Row, tile.Column, direction);

        public List<Tile> GetNeighbours(Tile tile, int radius) => _tiles.GetNeighbours(tile.Row, tile.Column, radius);

        /// <summary>
        /// Get all neighbouring tiles to the given tile within the band defined by fromRadius and toRadius, inclusive.
        /// </summary>
        public List<Tile> GetNeighbours(Tile tile, int fromRadius, int toRadius) => _tiles.GetNeighbours((HexCoords)tile, fromRadius, toRadius);

        #endregion

        #region Paths

        public void AddPath(string name, MapPath path) => _paths.Add(name, path);

        public IEnumerable<MapPath> Paths => _paths.Values;

        public MapPath GetPath(string pathName)
        {
            return _paths.ContainsKey(pathName) ? _paths[pathName] : null;
        }

        #endregion

        #region IEnumerable

        public IEnumerator<Tile> GetEnumerator() => _tiles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _tiles.GetEnumerator();

        #endregion
    }
}
