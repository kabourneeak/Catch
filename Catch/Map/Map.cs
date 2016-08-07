using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Catch.Services;
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
        private readonly HexGridCollection<Tile> _tiles; 
        private readonly Dictionary<string, MapPath> _paths;
        private readonly float _tileRadius;

        public Map(IConfig config, int rows, int columns)
        {
            _config = config;
            _tileRadius = config.GetFloat("TileRadius");

            Rows = rows;
            Columns = columns;
            Size = new Vector2((float)(Columns * _tileRadius * 1.5 + _tileRadius / 2), (float)(Rows * 2 * HexUtils.GetRadiusHeight(_tileRadius)));

            _tiles = new HexGridCollection<Tile>(Rows, Columns);
            _tiles.Populate((hc, v) => new Tile(hc, this, _config));

            _paths = new Dictionary<string, MapPath>();
        }

        #region Hexagonal Grid

        public int Rows { get; }
        public int Columns { get; }
        public Vector2 Size { get; }

        public Tile GetHex(HexCoords hc) => _tiles.GetHex(hc);

        public Tile GetNeighbour(Tile tile, HexDirection direction) => _tiles.GetNeighbour(tile.Coords, direction);

        public List<Tile> GetNeighbours(Tile tile, int radius) => _tiles.GetNeighbours(tile.Coords, radius);

        /// <summary>
        /// Get all neighbouring tiles to the given tile within the band defined by fromRadius and toRadius, inclusive.
        /// </summary>
        public List<Tile> GetNeighbours(Tile tile, int fromRadius, int toRadius) => _tiles.GetNeighbours(tile.Coords, fromRadius, toRadius);

        #endregion

        #region Point Location

        public HexCoords PointToHexCoords(Vector2 fieldCoords)
        {
            var q = fieldCoords.X * 2 / 3 / _tileRadius;
            var r = (-fieldCoords.X / 3 + HexUtils.SQRT3 / 3 * fieldCoords.Y) / _tileRadius;

            var x = q;
            var y = -q - r;
            var z = r;

            var rx = Math.Round(x);
            var ry = Math.Round(y);
            var rz = Math.Round(z);

            var xDiff = Math.Abs(rx - x);
            var yDiff = Math.Abs(ry - y);
            var zDiff = Math.Abs(rz - z);

            if (xDiff > yDiff && xDiff > zDiff)
                rx = -ry - rz;
            else if (yDiff > zDiff)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            return HexCoords.CreateFromCube((int) rx, (int) ry, (int) rz);
        }

        #endregion

        #region Paths

        public void AddPath(MapPath path) => _paths.Add(path.Name, path);

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
