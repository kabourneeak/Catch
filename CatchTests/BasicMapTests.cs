using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    public class BasicTileProvider : ITileProvider
    {
        public Tile CreateTile(int row, int col)
        {
            return new Tile(row, col, new CompiledConfig());
        }
    }

    public class MapAdapter : Map
    {
        public MapAdapter() : base(new BasicTileProvider(), new CompiledConfig())
        {
            
        }

        public List<Tile> GetTiles()
        {
            return Tiles;
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
            Assert.AreEqual(map.GetListOffset(0, 1), 3);
            Assert.AreEqual(map.GetListOffset(1, 1), 4);
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
            Assert.IsTrue(map.GetCoordsAreValid(0, 1));
            Assert.IsTrue(map.GetCoordsAreValid(1, 1));
            Assert.IsTrue(map.GetCoordsAreValid(0, 2));
            Assert.IsTrue(map.GetCoordsAreValid(1, 2));
            Assert.IsTrue(map.GetCoordsAreValid(2, 2));

            Assert.IsFalse(map.GetCoordsAreValid(-1, 0));
            Assert.IsFalse(map.GetCoordsAreValid(0, -1));

            Assert.IsFalse(map.GetCoordsAreValid(3, 0));
            Assert.IsFalse(map.GetCoordsAreValid(2, 1));
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
            Assert.AreEqual(tiles[i++], map.GetTile(0, 1));
            Assert.AreEqual(tiles[i++], map.GetTile(1, 1));
            Assert.AreEqual(tiles[i++], map.GetTile(0, 2));
            Assert.AreEqual(tiles[i++], map.GetTile(1, 2));
            Assert.AreEqual(tiles[i++], map.GetTile(2, 2));

            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(-1, 0));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(0, -1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(3, 0));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(2, 1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(3, 2));
            Assert.ThrowsException<IndexOutOfRangeException>(() => map.GetTile(0, 3));
        }

        [TestMethod]
        public void TestAllNeighbours()
        {
            var map = new MapAdapter();

            map.Initialize(6, 6);

            var center = map.GetTile(3, 3);

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(6, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 2)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 2)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(2, 3)));
        }

        [TestMethod]
        public void TestCornerNeighbours()
        {
            var map = new MapAdapter();

            map.Initialize(6, 6);

            var center = map.GetTile(0, 0);

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(2, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetTile(0, 1)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(1, 0)));
        }

        [TestMethod]
        public void TestRadius2Neighbours()
        {
            var map = new MapAdapter();

            map.Initialize(10, 10);

            var center = map.GetTile(5, 5);

            var neighbours = map.GetNeighbours(center, 2);

            Assert.AreEqual(12, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 6)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(5, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(5, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(6, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(7, 5)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(7, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(5, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(6, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(5, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(4, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetTile(3, 5)));
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
