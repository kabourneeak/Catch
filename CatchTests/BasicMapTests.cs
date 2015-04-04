using System;
using System.Collections.Generic;
using Catch.Base;
using Catch.Models;
using Catch.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    public class BasicTileProvider : IHexTileProvider
    {
        public IHexTile CreateTile(int row, int col)
        {
            return new BasicHexTile(row, col, new CompiledConfig());
        }
    }

    public class BasicMapAdapter : BasicMap
    {
        public BasicMapAdapter() : base(new BasicTileProvider(), new CompiledConfig())
        {
            
        }

        public List<IHexTile> GetTiles()
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
            var map = new BasicMapAdapter();

            map.Initialize(3, 3);

            var tiles = map.GetTiles();

            Assert.AreEqual(tiles.Count, 3 + 2 + 3);
        }

        [TestMethod]
        public void TestListOffset()
        {
            var map = new BasicMapAdapter();

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
            var map = new BasicMapAdapter();

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
            var map = new BasicMapAdapter();

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
    }
}
