using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Map;
using Catch.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    public class MapAdapter : Map
    {
        public MapAdapter() : base(new CompiledConfig())
        {
            
        }

        public List<Tile> GetTiles()
        {
            return Tiles;
        }

        public new HexCoords GetNeighbourCoords(int row, int col, TileDirection direction)
        {
            return base.GetNeighbourCoords(row, col, direction);
        }

        public new HexCoords GetNeighbourCoords(HexCoords coords, TileDirection direction)
        {
            return base.GetNeighbourCoords(coords, direction);
        }

        public new int GetListOffset(int row, int col)
        {
            return base.GetListOffset(row, col);
        }

        public new bool GetCoordsAreValid(int row, int col)
        {
            return base.GetCoordsAreValid(row, col);
        }
    }

    [TestClass]
    public class BasicMapTests
    {
        [TestMethod]
        public void TestSize()
        {
            var map = new MapAdapter();

            map.Initialize(3, 3);

            var tiles = map.GetTiles();

            Assert.AreEqual(tiles.Count, 3 + 2 + 3);
        }

        [TestMethod]
        public void TestListOffset()
        {
            var map = new MapAdapter();

            map.Initialize(3, 3);

            Assert.AreEqual(map.GetListOffset(0, 0), 0);
            Assert.AreEqual(map.GetListOffset(1, 0), 1);
            Assert.AreEqual(map.GetListOffset(2, 0), 2);
            // Assert.AreEqual(map.GetListOffset(0, 1), 3); // invalid coordinate
            Assert.AreEqual(map.GetListOffset(1, 1), 3);
            Assert.AreEqual(map.GetListOffset(2, 1), 4);
            Assert.AreEqual(map.GetListOffset(0, 2), 5);
            Assert.AreEqual(map.GetListOffset(1, 2), 6);
            Assert.AreEqual(map.GetListOffset(2, 2), 7);
        }

        [TestMethod]
        public void TestGetCoordsAreValid()
        {
            var map = new MapAdapter();

            map.Initialize(3, 3);

            Assert.IsTrue(map.GetCoordsAreValid(0, 0));
            Assert.IsTrue(map.GetCoordsAreValid(1, 0));
            Assert.IsTrue(map.GetCoordsAreValid(2, 0));
            Assert.IsTrue(map.GetCoordsAreValid(1, 1));
            Assert.IsTrue(map.GetCoordsAreValid(2, 1));
            Assert.IsTrue(map.GetCoordsAreValid(0, 2));
            Assert.IsTrue(map.GetCoordsAreValid(1, 2));
            Assert.IsTrue(map.GetCoordsAreValid(2, 2));

            // 0 row has no elements in odd columns
            Assert.IsFalse(map.GetCoordsAreValid(0, 1));

            Assert.IsFalse(map.GetCoordsAreValid(-1, 0));
            Assert.IsFalse(map.GetCoordsAreValid(0, -1));

            Assert.IsFalse(map.GetCoordsAreValid(3, 0));
            Assert.IsFalse(map.GetCoordsAreValid(3, 2));

            Assert.IsFalse(map.GetCoordsAreValid(3, 0));
            Assert.IsFalse(map.GetCoordsAreValid(3, 1));
            Assert.IsFalse(map.GetCoordsAreValid(3, 2));
            Assert.IsFalse(map.GetCoordsAreValid(3, 3));
        }

        [TestMethod]
        public void TestIndexing()
        {
            var map = new MapAdapter();

            map.Initialize(3, 3);

            var tiles = map.GetTiles();

            // this assumes that hexagons are generated column by column
            var i = 0;
            Assert.AreEqual(tiles[i++], map.GetTile(0, 0));
            Assert.AreEqual(tiles[i++], map.GetTile(1, 0));
            Assert.AreEqual(tiles[i++], map.GetTile(2, 0));
            Assert.AreEqual(tiles[i++], map.GetTile(1, 1));
            Assert.AreEqual(tiles[i++], map.GetTile(2, 1));
            Assert.AreEqual(tiles[i++], map.GetTile(0, 2));
            Assert.AreEqual(tiles[i++], map.GetTile(1, 2));
            Assert.AreEqual(tiles[i++], map.GetTile(2, 2));

            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(0, 1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(-1, 0));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(0, -1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(3, 0));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(3, 2));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(0, 3));
        }

        [TestMethod]
        public void TestDirection()
        {
            var map = new MapAdapter();

            map.Initialize(6, 6);

            var coords = new HexCoords() {Row = 4, Column = 3};

            coords = map.GetNeighbourCoords(coords, TileDirection.North);
            Assert.AreEqual(5, coords.Row);
            Assert.AreEqual(3, coords.Column);

            coords = map.GetNeighbourCoords(coords, TileDirection.SouthEast);
            Assert.AreEqual(4, coords.Row);
            Assert.AreEqual(4, coords.Column);

            coords = map.GetNeighbourCoords(coords, TileDirection.South);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(4, coords.Column);

            coords = map.GetNeighbourCoords(coords, TileDirection.SouthWest);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(3, coords.Column);

            coords = map.GetNeighbourCoords(coords, TileDirection.NorthWest);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(2, coords.Column);
            
            coords = map.GetNeighbourCoords(coords, TileDirection.North);
            Assert.AreEqual(4, coords.Row);
            Assert.AreEqual(2, coords.Column);

            coords = map.GetNeighbourCoords(coords, TileDirection.NorthEast);
            Assert.AreEqual(5, coords.Row);
            Assert.AreEqual(3, coords.Column);

        }


        [TestMethod]
        public void TestAllNeighbours()
        {
            var map = new MapAdapter();

            map.Initialize(8, 8);

            var center = map.GetTile(4, 3);

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(6, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetTile(5, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 2)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 2)));
        }

        [TestMethod]
        public void TestCornerNeighbours()
        {
            var map = new MapAdapter();

            map.Initialize(6, 6);

            var center = map.GetTile(0, 0);

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(2, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetTile(1, 0)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(1, 1)));
        }

        [TestMethod]
        public void TestRadius2Neighbours()
        {
            var map = new MapAdapter();

            map.Initialize(10, 10);

            var center = map.GetTile(5, 5);

            var neighbours = map.GetNeighbours(center, 2);

            Assert.AreEqual(12, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetTile(7, 5)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(6, 6)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(6, 7)));

            Assert.IsTrue(neighbours.Contains(map.GetTile(5, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 6)));
            
            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 5)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 3)));
            
            Assert.IsTrue(neighbours.Contains(map.GetTile(5, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(6, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(6, 4)));
        }

        [TestMethod]
        public void TestNoNeighbours()
        {
            var map = new MapAdapter();

            map.Initialize(1, 1);

            var center = map.GetTile(0, 0);

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(0, neighbours.Count);
        }
    }
}
