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
        private readonly ITileProvider _tileProvider;
        private readonly IConfig _config;

        public Map(ITileProvider tileProvider, IConfig config)
        {
            _tileProvider = tileProvider;
            _config = config;

            _tileRadius = config.GetFloat("TileRadius");
        }

        #region Map implementation

        protected List<Tile> Tiles;
        protected Dictionary<string, MapPath> Paths;
        private float _tileRadius;

        public void Initialize(int rows, int columns)
        {
            Assert(rows >= 1);
            Assert(columns >= 1);

            Tiles = new List<Tile>();
            Paths = new Dictionary<string, MapPath>();

            Rows = rows;
            Columns = columns;

            for (var col = 0; col < columns; ++col)
            {
                // the odd nummbered columns have one fewer row
                for (var row = 0; row < rows - (col % 2); ++row)
                {
                    var tile = _tileProvider.CreateTile(row, col);
                    Tiles.Add(tile);
                }
            }

            Size = new Vector2((float)(Columns * _tileRadius * 1.5 + _tileRadius / 2), (float)(Rows * 2 * HexUtils.GetRadiusHeight(_tileRadius)));
        }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public Vector2 Size { get; private set; }

        public Tile GetTile(int row, int col)
        {
            Assert(row >= 0);
            Assert(col >= 0);

            if (GetCoordsAreValid(row, col))
            {
                return Tiles[GetListOffset(row, col)];
            }

            throw new IndexOutOfRangeException(string.Format("Row/Column ({0},{1}) are invalid for a grid of size ({2},{3}).", row, col, Rows, Columns));
        }

        public bool HasNeighbour(Tile tile, TileDirection direction)
        {
            Assert(tile != null);
            Assert(Tiles.Contains(tile));

            return GetNeighbourCoords(tile.Row, tile.Column, direction).Valid;
        }

        public Tile GetNeighbour(Tile tile, TileDirection direction)
        {
            Assert(tile != null);
            Assert(Tiles.Contains(tile));
            Assert(GetCoordsAreValid(tile.Row, tile.Column));

            var coords = GetNeighbourCoords(tile.Row, tile.Column, direction);

            if (coords.Valid)
            {
                return GetTile(coords.Row, coords.Column);
            }

            return null;
        }

        public List<Tile> GetNeighbours(Tile tile)
        {
            Assert(tile != null);
            Assert(Tiles.Contains(tile));
            Assert(GetCoordsAreValid(tile.Row, tile.Column));

            var neighbours = from direction in TileDirectionExtensions.AllTileDirections
                let n = GetNeighbour(tile, direction)
                where n != null
                select n;

            return neighbours.ToList();
        }

        public List<Tile> GetNeighbours(Tile tile, int radius)
        {
            Assert(tile != null);
            Assert(Tiles.Contains(tile));
            Assert(GetCoordsAreValid(tile.Row, tile.Column));
            Assert(radius >= 1);

            throw new NotImplementedException();
        }

        public MapPath GetPath(string pathName)
        {
            if (Paths.ContainsKey(pathName))
            {
                return Paths[pathName];
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Calculates the row and column of the neighbout in the given 
        /// direction from the given coordinates. Also returns true or false
        /// depending on if those coordinates are valid.
        /// </summary>
        /// <param name="row">The origin row</param>
        /// <param name="col">The origin column</param>
        /// <param name="direction">The direction of the neighbour</param>
        /// <param name="rows">The total number of rows in the grid</param>
        /// <param name="columns">The total number of columns in the grid</param>
        /// <returns></returns>
        protected HexCoords GetNeighbourCoords(int row, int col, TileDirection direction)
        {
            Assert(row >= 0);
            Assert(col >= 0);
            Assert(row < Rows - col % 2);
            Assert(col < Columns);

            var coords = new HexCoords {Valid = false};

            switch (direction)
            {
                case TileDirection.North:
                    coords.Row = row - 1;
                    coords.Column = col;
                    break;
                case TileDirection.NorthEast:
                    coords.Row = row - (1 - col % 2); // even column moves up a row
                    coords.Column = col + 1;
                    break;
                case TileDirection.SouthEast:
                    coords.Row = row + (col % 2); // even column stays in same row
                    coords.Column = col + 1;
                    break;
                case TileDirection.South:
                    coords.Row = row + 1;
                    coords.Column = col;
                    break;
                case TileDirection.SouthWest:
                    coords.Row = row + (col % 2); // even column stays in same row
                    coords.Column = col - 1;
                    break;
                case TileDirection.NorthWest:
                    coords.Row = row - (1 - col % 2); // even column moves up a row
                    coords.Column = col - 1;
                    break;
            }

            coords.Valid = GetCoordsAreValid(coords.Row, coords.Column);

            return coords;
        }

        protected bool GetCoordsAreValid(int row, int col)
        {
            if (col >= 0 && col < Columns)
            {
                if (row >= 0 && row < Rows - (col % 2))
                {
                    return true;
                }
            }

            return false;            
        }

        protected int GetListOffset(int row, int col)
        {
            Assert(row >= 0);
            Assert(col >= 0);
            Assert(row < Rows - col % 2);
            Assert(col < Columns);

            return (col * Rows) - (col / 2) + row;
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            return Tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [Conditional("DEBUG")]
        private static void Assert(bool condition)
        {
            if (!condition)
                Debugger.Break();
        }
    }
}
