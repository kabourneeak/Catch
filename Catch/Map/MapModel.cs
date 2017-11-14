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
        private readonly float _tileRadius;

        private HexGridCollection<MapTileModel> _tiles;
        private MapTileModel _offMapTile;
        private Dictionary<string, MapPathModel> _paths;

        public MapModel(IConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _tileRadius = config.GetFloat(CoreConfig.TileRadius);

            Rows = 0;
            Columns = 0;
            Size = Vector2.Zero;
        }

        #region IMap Implementation

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Vector2 Size { get; private set; }

        public IEnumerable<IMapTile> Tiles => _tiles;

        public IMapTile OffMapTile => _offMapTile;

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

        public void Initialize(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Size = new Vector2((float)(Columns * _tileRadius * 1.5 + _tileRadius / 2), Rows * 2 * HexUtils.GetRadiusHeight(_tileRadius));

            _tiles = new HexGridCollection<MapTileModel>(Rows, Columns);
            _paths = new Dictionary<string, MapPathModel>();
        }

        public void Populate(Func<HexCoords, MapTileModel, MapTileModel> populator)
        {
            _tiles.Populate(populator);
            _offMapTile = populator(HexCoords.CreateFromOffset(-100, -100), default(MapTileModel));
        }

        public void AddPath(MapPathModel pathModel) => _paths.Add(pathModel.Name, pathModel);

        public IEnumerable<MapTileModel> TileModels => _tiles;

        public MapTileModel GetTileModel(IMapTile tile) => _tiles.GetHex(tile.Coords);

        public MapTileModel GetTileModel(HexCoords hc) => _tiles.GetHex(hc);

        public MapTileModel OffMapTileModel => _offMapTile;

        #endregion
    }
}
