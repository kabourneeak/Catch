using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Catch.Models;

namespace Catch.Base
{
    /// <summary>
    /// Implements the core functionality of IMap.  Specifically, methods for 
    /// navigating the hexagonal grid coordinate system are found here, and
    /// may be useful to derived classes.
    /// </summary>
    public abstract class BasicMap : IMap
    {
        protected List<IHexTile> Tiles;
        protected abstract IHexTileProvider TileProvider { get; set; }
        protected Dictionary<string, IPath> Paths;

        protected BasicMap()
        {
        }

        public void Initialize(int rows, int columns)
        {
            Debug.Assert(rows >= 1);
            Debug.Assert(columns >= 1);

            Tiles = new List<IHexTile>();
            Paths = new Dictionary<string, IPath>();

            Rows = rows;
            Columns = columns;

            for (var col = 0; col < columns; ++col)
            {
                // the odd nummbered columns have one fewer row
                for (var row = 0; row < rows - (col % 2); ++row)
                {
                    var tile = TileProvider.CreateTile(row, col);
                    Tiles.Add(tile);
                }
            }
        }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public IHexTile GetTile(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(col >= 0);

            if (GetCoordsAreValid(row, col))
            {
                return Tiles[GetListOffset(row, col)];
            }

            throw new IndexOutOfRangeException(string.Format("Row/Column ({0},{1}) are invalid for a grid of size ({2},{3}).", row, col, Rows, Columns));
        }

        public bool HasNeighbour(IHexTile tile, TileDirection direction)
        {
            Debug.Assert(tile != null);
            Debug.Assert(Tiles.Contains(tile));

            return GetNeighbourCoords(tile.Row, tile.Column, direction).Valid;
        }

        public IHexTile GetNeighbour(IHexTile tile, TileDirection direction)
        {
            Debug.Assert(tile != null);
            Debug.Assert(Tiles.Contains(tile));
            Debug.Assert(GetCoordsAreValid(tile.Row, tile.Column));

            var coords = GetNeighbourCoords(tile.Row, tile.Column, direction);

            if (coords.Valid)
            {
                return GetTile(coords.Row, coords.Column);
            }

            return null;
        }

        public List<IHexTile> GetNeighbours(IHexTile tile)
        {
            Debug.Assert(tile != null);
            Debug.Assert(Tiles.Contains(tile));
            Debug.Assert(GetCoordsAreValid(tile.Row, tile.Column));

            var neighbours = from direction in TileDirectionExtensions.AllTileDirections
                let n = GetNeighbour(tile, direction)
                where n != null
                select n;

            return neighbours.ToList();
        }

        public List<IHexTile> GetNeighbours(IHexTile tile, int radius)
        {
            Debug.Assert(tile != null);
            Debug.Assert(Tiles.Contains(tile));
            Debug.Assert(GetCoordsAreValid(tile.Row, tile.Column));
            Debug.Assert(radius >= 1);

            throw new NotImplementedException();
        }

        public IPath GetPath(string pathName)
        {
            if (Paths.ContainsKey(pathName))
            {
                return Paths[pathName];
            }

            return null;
        }

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
            Debug.Assert(row >= 0);
            Debug.Assert(col >= 0);
            Debug.Assert(row < Rows - col % 2);
            Debug.Assert(col < Columns);

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
            Debug.Assert(row >= 0);
            Debug.Assert(col >= 0);
            Debug.Assert(row < Rows - col % 2);
            Debug.Assert(col < Columns);

            return (col * Rows) - (col / 2) + row;
        }
    }
}
