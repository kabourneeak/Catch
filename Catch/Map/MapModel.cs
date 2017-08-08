using System;
using System.Collections.Generic;
using System.Numerics;
using Catch.Base;
using Catch.Services;
using CatchLibrary.HexGrid;

namespace Catch.Map
{
    /// <summary>
    /// Implements a "living" map or field of play, which has tiles, towers, 
    /// paths.
    /// </summary>
    public class MapModel : IMap
    {
        private readonly IConfig _config;
        private readonly HexGridCollection<MapTileModel> _tiles; 
        private readonly Dictionary<string, MapPathModel> _paths;
        private readonly float _tileRadius;

        public MapModel(IConfig config, int rows, int columns)
        {
            _config = config;
            _tileRadius = config.GetFloat("TileRadius");

            Rows = rows;
            Columns = columns;
            Size = new Vector2((float)(Columns * _tileRadius * 1.5 + _tileRadius / 2), Rows * 2 * HexUtils.GetRadiusHeight(_tileRadius));

            _tiles = new HexGridCollection<MapTileModel>(Rows, Columns);
            _tiles.Populate((hc, v) => new MapTileModel(hc, _config));

            _paths = new Dictionary<string, MapPathModel>();
        }

        #region IMap Implementation

        public int Rows { get; }
        public int Columns { get; }
        public Vector2 Size { get; }

        public IEnumerable<IMapTile> Tiles => _tiles;

        public bool HasHex(HexCoords hc) => _tiles.HasHex(hc);

        public IMapTile GetTile(HexCoords hc) => _tiles.GetHex(hc);

        public IMapTile GetNeighbour(IMapTile tile, HexDirection direction) => _tiles.GetNeighbour(tile.Coords, direction);

        public IEnumerable<IMapTile> GetNeighbours(IMapTile tile, int radius) => _tiles.GetNeighbours(tile.Coords, radius);

        public IEnumerable<IMapTile> GetNeighbours(IMapTile tile, int fromRadius, int toRadius) => _tiles.GetNeighbours(tile.Coords, fromRadius, toRadius);

        #endregion

        #region Paths

        public IEnumerable<IMapPath> Paths => _paths.Values;

        public IMapPath GetPath(string pathName) => _paths.ContainsKey(pathName) ? _paths[pathName] : null;

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

            return HexCoords.CreateFromCube((int)rx, (int)ry, (int)rz);
        }

        #endregion

        #region Management

        public void AddPath(MapPathModel pathModel) => _paths.Add(pathModel.Name, pathModel);

        public IEnumerable<MapTileModel> TileModels => _tiles;

        public MapTileModel GetTileModel(IMapTile tile) => _tiles.GetHex(tile.Coords);

        public MapTileModel GetTileModel(HexCoords hc) => _tiles.GetHex(hc);

        #endregion
    }
}
