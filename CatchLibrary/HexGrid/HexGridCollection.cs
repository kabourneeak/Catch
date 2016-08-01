﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace CatchLibrary.HexGrid
{
    /// <summary>
    /// Implements a hexagonal grid. A grid is always a full rectangle -- there 
    /// are no holes in the middle, however, the grid is not a proper rectangle
    /// owing the hex shape. The even-numbered columns have one extra hexagon
    /// compared to the odd-numbered ones.
    /// 
    /// Methods for navigating the hexagonal grid coordinate system are found 
    /// here.
    /// </summary>
    public class HexGridCollection<T> : IEnumerable<T> where T : class
    {
        public int Rows { get; }
        public int Columns { get; }
        public int Count => Hexes.Count;

        protected readonly List<T> Hexes;

        #region Construction

        public HexGridCollection(int rows, int columns)
        {
            DebugUtils.Assert(rows >= 1);
            DebugUtils.Assert(columns >= 1);

            Rows = rows;
            Columns = columns;

            // TODO set initial capacity
            Hexes = new List<T>();

            // fill out collection
            for (var col = 0; col < columns; ++col)
            {
                // the odd nummbered columns have one fewer row
                for (var row = 0 + col.Mod(2); row < rows; ++row)
                {
                    Hexes.Add(null);
                }
            }
        }

        #endregion

        #region Setters

        public void SetHex(HexCoords hexCoords, T value)
        {
            SetHex(hexCoords.Row, hexCoords.Column, value);
        }

        public void SetHex(int row, int column, T value)
        {
            DebugUtils.Assert(row >= 0);
            DebugUtils.Assert(column >= 0);

            if (GetCoordsAreValid(row, column))
            {
                Hexes[GetListOffset(row, column)] = value;
                return;
            }

            throw new IndexOutOfRangeException(
                $"Row/Column ({row},{column}) are invalid for a grid of size ({Rows},{Columns}).");
        }

        /// <summary>
        /// Iterates over the rows and columns of the grid and stores the returned value
        /// from the delegate function at that position.
        /// </summary>
        /// <param name="populator"></param>
        public void Populate(HexGridPopulator<T> populator)
        {
            for (var column = 0; column < Columns; ++column)
            {
                // the odd nummbered columns have one fewer row
                for (var row = 0 + column.Mod(2); row < Rows; ++row)
                {
                    var offset = GetListOffset(row, column);
                    var curVal = Hexes[offset];
                    Hexes[offset] = populator(row, column, curVal);
                }
            }
        }

        #endregion

        #region IEnumerable

        public bool Contains(T obj) => Hexes.Contains(obj);

        public IEnumerator<T> GetEnumerator()
        {
            return Hexes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Getters

        public T GetHex(HexCoords hexCoords)
        {
            return GetHex(hexCoords.Row, hexCoords.Column);
        }

        public T GetHex(int row, int column)
        {
            DebugUtils.Assert(row >= 0);
            DebugUtils.Assert(column >= 0);

            if (GetCoordsAreValid(row, column))
            {
                return Hexes[GetListOffset(row, column)];
            }

            throw new IndexOutOfRangeException(
                $"Row/Column ({row},{column}) are invalid for a grid of size ({Rows},{Columns}).");
        }

        public bool HasNeighbour(HexCoords hexCoords, HexDirection direction)
        {
            return GetNeighbourCoords(hexCoords.Row, hexCoords.Column, direction).Valid;
        }

        public bool HasNeighbour(int row, int column, HexDirection direction)
        {
            return GetNeighbourCoords(row, column, direction).Valid;
        }

        public T GetNeighbour(HexCoords hexCoords, HexDirection direction)
        {
            return GetNeighbour(hexCoords.Row, hexCoords.Column, direction);
        }

        public T GetNeighbour(int row, int column, HexDirection direction)
        {
            DebugUtils.Assert(GetCoordsAreValid(row, column));

            var coords = GetNeighbourCoords(row, column, direction);

            return coords.Valid ? GetHex(coords.Row, coords.Column) : null;
        }

        private static readonly HexDirection[] clockwiseWalk =
        {
            HexDirection.SouthEast,
            HexDirection.South,
            HexDirection.SouthWest,
            HexDirection.NorthWest,
            HexDirection.North,
            HexDirection.NorthEast
        };

        public List<T> GetNeighbours(HexCoords hexCoords, int radius)
        {
            DebugUtils.Assert(GetCoordsAreValid(hexCoords.Row, hexCoords.Column));
            DebugUtils.Assert(radius >= 1);

            var neighbours = new List<T>();

            // walk north by radius
            for (var i = 0; i < radius; ++i)
                hexCoords = GetNeighbourCoords(hexCoords, HexDirection.North);

            // walk around clockwise
            foreach (var d in clockwiseWalk)
            {
                for (var i = 0; i < radius; ++i)
                {
                    hexCoords = GetNeighbourCoords(hexCoords, d);
                    if (hexCoords.Valid)
                        neighbours.Add(GetHex(hexCoords));
                }
            }

            return neighbours;
        }

        public List<T> GetNeighbours(int row, int column, int radius)
        {
            return GetNeighbours(new HexCoords {Row = row, Column = column, Valid = true}, radius);
        }

        /// <summary>
        /// Get all immediately neighbouring tiles to the given tile (i.e., radius equals 1)
        /// </summary>
        public List<T> GetNeighbours(HexCoords hexCoords)
        {
            return GetNeighbours(hexCoords, 1);
        }

        public List<T> GetNeighbours(int row, int column)
        {
            return GetNeighbours(row, column, 1);
        }

        /// <summary>
        /// Get all neighbouring tiles to the given tile within the band defined by fromRadius and toRadius, inclusive.
        /// </summary>
        public List<T> GetNeighbours(HexCoords hexCoords, int fromRadius, int toRadius)
        {
            DebugUtils.Assert(1 <= fromRadius);
            DebugUtils.Assert(fromRadius <= toRadius);

            var neighbours = GetNeighbours(hexCoords, fromRadius);

            for (var i = fromRadius + 1; i <= toRadius; ++i)
                neighbours.AddRange(GetNeighbours(hexCoords, i));

            return neighbours;
        }

        protected HexCoords GetNeighbourCoords(HexCoords hexCoords, HexDirection direction)
        {
            return GetNeighbourCoords(hexCoords.Row, hexCoords.Column, direction);
        }

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
        protected HexCoords GetNeighbourCoords(int row, int col, HexDirection direction)
        {
            var coords = new HexCoords {Valid = false};

            switch (direction)
            {
                case HexDirection.North:
                    coords.Row = row + 1;
                    coords.Column = col;
                    break;
                case HexDirection.NorthEast:
                    coords.Row = row + (1 - col.Mod(2)); // if currently in even column, move up a row
                    coords.Column = col + 1;
                    break;
                case HexDirection.SouthEast:
                    coords.Row = row - (col.Mod(2)); // if currently in even column, stay in same row
                    coords.Column = col + 1;
                    break;
                case HexDirection.South:
                    coords.Row = row - 1;
                    coords.Column = col;
                    break;
                case HexDirection.SouthWest:
                    coords.Row = row - (col.Mod(2)); // if currently in even column, stay in same row
                    coords.Column = col - 1;
                    break;
                case HexDirection.NorthWest:
                    coords.Row = row + (1 - col.Mod(2)); // if currently in even column, move up a row
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

        #endregion
    }
}