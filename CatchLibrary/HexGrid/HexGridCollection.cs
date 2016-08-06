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
                    var hexCoords = HexCoords.CreateFromOffset(row, column);
                    var offset = GetListOffset(hexCoords);
                    var curVal = Hexes[offset];
                    Hexes[offset] = populator(hexCoords, curVal);
                }
            }
        }

        #endregion

        #region Basic Accessors

        public T this[HexCoords hc]
        {
            get { return GetHex(hc); }
            set { SetHex(hc, value);}
        }

        public void SetHex(HexCoords hc, T value)
        {
            if (IsInCollection(hc))
            {
                Hexes[GetListOffset(hc)] = value;
            }
            else
            {
                throw new IndexOutOfRangeException($"{hc} are not present in this collection");
            }
        }

        public T GetHex(HexCoords hc)
        {
            if (IsInCollection(hc))
            {
                return Hexes[GetListOffset(hc)];
            }

            throw new IndexOutOfRangeException($"{hc} are not present in this collection");
        }

        #endregion

        #region Neighbour Accessors

        public bool HasNeighbour(HexCoords hc, HexDirection direction)
        {
            return IsInCollection(GetNeighbourCoords(hc, direction));
        }

        public T GetNeighbour(HexCoords hc, HexDirection direction)
        {
            var neighbourHc = GetNeighbourCoords(hc, direction);

            return IsInCollection(neighbourHc) ? GetHex(neighbourHc) : null;
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

        public List<T> GetNeighbours(HexCoords hc, int radius)
        {
            DebugUtils.Assert(IsInCollection(hc));
            DebugUtils.Assert(radius >= 1);

            var neighbours = new List<T>();

            // walk north by radius
            for (var i = 0; i < radius; ++i)
                hc = GetNeighbourCoords(hc, HexDirection.North);

            // walk around clockwise
            foreach (var d in clockwiseWalk)
            {
                for (var i = 0; i < radius; ++i)
                {
                    hc = GetNeighbourCoords(hc, d);
                    if (IsInCollection(hc))
                        neighbours.Add(GetHex(hc));
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Get all immediately neighbouring hexes to the given hex (i.e., radius equals 1)
        /// </summary>
        public List<T> GetNeighbours(HexCoords hc)
        {
            return GetNeighbours(hc, 1);
        }

        /// <summary>
        /// Get all neighbouring hexes to the given hex within the band defined by fromRadius and toRadius, inclusive.
        /// </summary>
        public List<T> GetNeighbours(HexCoords hc, int fromRadius, int toRadius)
        {
            DebugUtils.Assert(1 <= fromRadius);
            DebugUtils.Assert(fromRadius <= toRadius);

            var neighbours = GetNeighbours(hc, fromRadius);

            for (var i = fromRadius + 1; i <= toRadius; ++i)
                neighbours.AddRange(GetNeighbours(hc, i));

            return neighbours;
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

        #region Storage

        protected readonly List<T> Hexes;

        /// <summary>
        /// Calculates the neighbouring coordinates the given direction from the
        /// given coordinates, whether or not those coordinates are valid in this
        /// collection.
        /// </summary>
        /// <param name="hc">The origin coordinates</param>
        /// <param name="direction">The direction of the neighbour</param>
        /// <returns>A HexCoords object containing the results</returns>
        protected HexCoords GetNeighbourCoords(HexCoords hc, HexDirection direction)
        {
            switch (direction)
            {
                case HexDirection.North:
                    return HexCoords.CreateFromAxial(hc.Q, hc.R + 1);
                case HexDirection.NorthWest:
                    return HexCoords.CreateFromAxial(hc.Q - 1, hc.R + 1);
                case HexDirection.SouthWest:
                    return HexCoords.CreateFromAxial(hc.Q - 1, hc.R);
                case HexDirection.South:
                    return HexCoords.CreateFromAxial(hc.Q, hc.R - 1);
                case HexDirection.SouthEast:
                    return HexCoords.CreateFromAxial(hc.Q + 1, hc.R - 1);
                case HexDirection.NorthEast:
                    return HexCoords.CreateFromAxial(hc.Q + 1, hc.R);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        protected bool IsInCollection(HexCoords hc)
        {
            return (hc.Column >= 0 && hc.Column < Columns && hc.Row >= 0 && hc.Row < Rows);
        }

        protected int GetListOffset(HexCoords hc)
        {
            return (hc.Column * Rows) + hc.Row;
        }

        #endregion
    }
}
