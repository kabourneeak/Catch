using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Catch.Services;

namespace Catch.Base
{
    /// <summary>
    /// Implements a hex tile grid, which has tiles, towers, 
    /// paths. A grid is always a full rectangle -- there are no missing 
    /// instances in the middle.  However, the grid is not a proper rectangle
    /// owing the hex shape. The even-numbered columns have one extra hexagon
    /// compared to the odd-numbered ones.
    /// 
    /// Methods for navigating the hexagonal grid coordinate system are found 
    /// here.
    /// </summary>
    public class Map : IEnumerable<Tile>
    {
        private readonly IConfig _config;
        protected List<Tile> Tiles;
        private Dictionary<string, MapPath> _paths;
        private float _tileRadius;

        public Map(IConfig config)
        {
            _config = config;

            _tileRadius = config.GetFloat("TileRadius");
        }

        #region Map implementation

        public void Initialize(int rows, int columns)
        {
            DebugUtils.Assert(rows >= 1);
            DebugUtils.Assert(columns >= 1);

            Tiles = new List<Tile>();
            _paths = new Dictionary<string, MapPath>();

            Rows = rows;
            Columns = columns;

            for (var col = 0; col < columns; ++col)
            {
                // the odd nummbered columns have one fewer row
                for (var row = 0 + col.Mod(2); row < rows; ++row)
                {
                    Tiles.Add(new Tile(row, col, this, _config));
                }
            }

            Size = new Vector2((float)(Columns * _tileRadius * 1.5 + _tileRadius / 2), (float)(Rows * 2 * HexUtils.GetRadiusHeight(_tileRadius)));
        }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public Vector2 Size { get; private set; }

        public Tile GetTile(int row, int col)
        {
            DebugUtils.Assert(row >= 0);
            DebugUtils.Assert(col >= 0);

            if (GetCoordsAreValid(row, col))
            {
                return Tiles[GetListOffset(row, col)];
            }

            throw new IndexOutOfRangeException(string.Format("Row/Column ({0},{1}) are invalid for a grid of size ({2},{3}).", row, col, Rows, Columns));
        }

        protected Tile GetTile(HexCoords coords)
        {
            return GetTile(coords.Row, coords.Column);
        }

        public bool HasNeighbour(Tile tile, TileDirection direction)
        {
            DebugUtils.Assert(tile != null);
            DebugUtils.Assert(Tiles.Contains(tile));

            return GetNeighbourCoords(tile.Row, tile.Column, direction).Valid;
        }

        public Tile GetNeighbour(Tile tile, TileDirection direction)
        {
            DebugUtils.Assert(tile != null);
            DebugUtils.Assert(Tiles.Contains(tile));
            DebugUtils.Assert(GetCoordsAreValid(tile.Row, tile.Column));

            var coords = GetNeighbourCoords(tile.Row, tile.Column, direction);

            if (coords.Valid)
            {
                return GetTile(coords.Row, coords.Column);
            }

            return null;
        }

        private static readonly TileDirection[] clockwiseWalk = {
                TileDirection.SouthEast,
                TileDirection.South,
                TileDirection.SouthWest,
                TileDirection.NorthWest,
                TileDirection.North,
                TileDirection.NorthEast
            };

        public List<Tile> GetNeighbours(Tile tile, int radius)
        {
            DebugUtils.Assert(tile != null);
            DebugUtils.Assert(Tiles.Contains(tile));
            DebugUtils.Assert(GetCoordsAreValid(tile.Row, tile.Column));
            DebugUtils.Assert(radius >= 1);

            var neighbours = new List<Tile>();
            var coords = new HexCoords() {Row = tile.Row, Column = tile.Column, Valid = true};

            // walk north by radius
            for (var i = 0; i < radius; ++i)
                coords = GetNeighbourCoords(coords, TileDirection.North);

            // walk around clockwise
            foreach (var d in clockwiseWalk)
            {
                for (var i = 0; i < radius; ++i)
                {
                    coords = GetNeighbourCoords(coords, d);
                    if (coords.Valid)
                        neighbours.Add(GetTile(coords));
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Get all immediately neighbouring tiles to the given tile (i.e., radius equals 1)
        /// </summary>
        public List<Tile> GetNeighbours(Tile tile)
        {
            return GetNeighbours(tile, 1);
        }

        /// <summary>
        /// Get all neighbouring tiles to the given tile within the band defined by fromRadius and toRadius, inclusive.
        /// </summary>
        public List<Tile> GetNeighbours(Tile tile, int fromRadius, int toRadius)
        {
            DebugUtils.Assert(1 <= fromRadius);
            DebugUtils.Assert(fromRadius <= toRadius);

            var neighbours = GetNeighbours(tile, fromRadius);

            for (var i = fromRadius + 1; i <= toRadius; ++i)
                neighbours.AddRange(GetNeighbours(tile, i));

            return neighbours;
        }

        #endregion

        #region Paths

        public void AddPath(string name, MapPath path)
        {
            _paths.Add(name, path);
        }

        public IEnumerable<MapPath> Paths { get { return _paths.Values; } }

        public MapPath GetPath(string pathName)
        {
            return _paths.ContainsKey(pathName) ? _paths[pathName] : null;
        }

        #endregion

        /// <summary>
        /// Calculates the row and column of the neighbour in the given direction from the
        /// given coordinates, whether or not those coordinates are valid on this map.
        /// </summary>
        /// <param name="row">The origin row</param>
        /// <param name="col">The origin column</param>
        /// <param name="direction">The direction of the neighbour</param>
        /// <param name="rows">The total number of rows in the grid</param>
        /// <param name="columns">The total number of columns in the grid</param>
        /// <returns>A HexCoords object containing the results</returns>
        protected HexCoords GetNeighbourCoords(int row, int col, TileDirection direction)
        {
            var coords = new HexCoords {Valid = false};

            switch (direction)
            {
                case TileDirection.North:
                    coords.Row = row + 1;
                    coords.Column = col;
                    break;
                case TileDirection.NorthEast:
                    coords.Row = row + (1 - col.Mod(2)); // if currently in even column, move up a row
                    coords.Column = col + 1;
                    break;
                case TileDirection.SouthEast:
                    coords.Row = row - (col.Mod(2)); // if currently in even column, stay in same row
                    coords.Column = col + 1;
                    break;
                case TileDirection.South:
                    coords.Row = row - 1;
                    coords.Column = col;
                    break;
                case TileDirection.SouthWest:
                    coords.Row = row - (col.Mod(2)); // if currently in even column, stay in same row
                    coords.Column = col - 1;
                    break;
                case TileDirection.NorthWest:
                    coords.Row = row + (1 - col.Mod(2)); // if currently in even column, move up a row
                    coords.Column = col - 1;
                    break;
            }

            coords.Valid = GetCoordsAreValid(coords.Row, coords.Column);

            return coords;
        }

        protected HexCoords GetNeighbourCoords(HexCoords coords, TileDirection direction)
        {
            return GetNeighbourCoords(coords.Row, coords.Column, direction);
        }

        protected bool GetCoordsAreValid(int row, int col)
        {
            if (col >= 0 && col < Columns)
            {
                if (row >= 0 + col.Mod(2) && row < Rows)
                {
                    return true;
                }
            }

            return false;            
        }

        protected int GetListOffset(int row, int col)
        {
            DebugUtils.Assert(row >= 0 + col.Mod(2));
            DebugUtils.Assert(col >= 0);
            DebugUtils.Assert(row < Rows);
            DebugUtils.Assert(col < Columns);

            return (col * Rows) - (col / 2) + (row - col.Mod(2));
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            return Tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
