using System;
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

            Hexes = new List<T>(Rows * Columns);

            // fill out collection
            for (var row = 0; row < Rows; ++row)
            {
                for (var col = 0; col < Columns; ++col)
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

            if (IsInCollection(row, column))
            {
                Hexes[GetListOffset(row, column)] = value;
            }
            else
            {
                throw new IndexOutOfRangeException(
                    $"Row/Column ({row},{column}) are invalid for a grid of size ({Rows},{Columns}).");
            }
        }

        /// <summary>
        /// Iterates over the rows and columns of the grid and stores the returned value
        /// from the delegate function at that position.
        /// </summary>
        /// <param name="populator"></param>
        public void Populate(HexGridPopulator<T> populator)
        {
            for (var row = 0; row < Rows; ++row)
            {
                for (var column = 0; column < Columns; ++column)
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

            if (IsInCollection(row, column))
            {
                return Hexes[GetListOffset(row, column)];
            }

            throw new IndexOutOfRangeException(
                $"Row/Column ({row},{column}) are invalid for a grid of size ({Rows},{Columns}).");
        }

        public bool HasNeighbour(HexCoords hexCoords, HexDirection direction)
        {
            return IsInCollection(GetNeighbourCoords(hexCoords.Row, hexCoords.Column, direction));
        }

        public bool HasNeighbour(int row, int column, HexDirection direction)
        {
            return IsInCollection(GetNeighbourCoords(row, column, direction));
        }

        public T GetNeighbour(HexCoords hexCoords, HexDirection direction)
        {
            return GetNeighbour(hexCoords.Row, hexCoords.Column, direction);
        }

        public T GetNeighbour(int row, int column, HexDirection direction)
        {
            DebugUtils.Assert(IsInCollection(row, column));

            var coords = GetNeighbourCoords(row, column, direction);

            return IsInCollection(coords) ? GetHex(coords.Row, coords.Column) : null;
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
            DebugUtils.Assert(IsInCollection(hexCoords.Row, hexCoords.Column));
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
                    if (IsInCollection(hexCoords))
                        neighbours.Add(GetHex(hexCoords));
                }
            }

            return neighbours;
        }

        public List<T> GetNeighbours(int row, int column, int radius)
        {
            return GetNeighbours(new HexCoords {Row = row, Column = column}, radius);
        }

        /// <summary>
        /// Get all immediately neighbouring hexes to the given hex (i.e., radius equals 1)
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
        /// Get all neighbouring hexes to the given hex within the band defined by fromRadius and toRadius, inclusive.
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
        /// <returns>A HexCoords object containing the results</returns>
        protected HexCoords GetNeighbourCoords(int row, int col, HexDirection direction)
        {
            switch (direction)
            {
                case HexDirection.North:
                    return new HexCoords() {Row = row + 1, Column = col};
                case HexDirection.NorthEast:
                    // if currently in even column, move up a row
                    return new HexCoords() {Row = row + (1 - (col & 1)), Column = col + 1};
                case HexDirection.SouthEast:
                    // if currently in even column, stay in same row
                    return new HexCoords() {Row = row - (col & 1), Column = col + 1};
                case HexDirection.South:
                    return new HexCoords() {Row = row - 1, Column = col};
                case HexDirection.SouthWest:
                    // if currently in even column, stay in same row
                    return new HexCoords() { Row = row - (col & 1), Column = col - 1};
                case HexDirection.NorthWest:
                    // if currently in even column, move up a row
                    return new HexCoords() {Row = row + (1 - (col & 1)), Column = col - 1};
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }

        protected bool IsInCollection(HexCoords hc)
        {
            return IsInCollection(hc.Row, hc.Column);
        }

        protected bool IsInCollection(int row, int col)
        {
            return (col >= 0 && col < Columns && row >= 0 && row < Rows);
        }

        protected int GetListOffset(int row, int col)
        {
            DebugUtils.Assert(row >= 0);
            DebugUtils.Assert(col >= 0);
            DebugUtils.Assert(row < Rows);
            DebugUtils.Assert(col < Columns);

            return (col * Rows) + row;
        }

        #endregion
    }
}
