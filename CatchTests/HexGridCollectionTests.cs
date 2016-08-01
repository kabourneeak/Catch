using System;
using System.Collections.Generic;
using System.Numerics;
using CatchLibrary.HexGrid;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    public class HexGridCollectionAdapter : HexGridCollection<object>
    {
        public HexGridCollectionAdapter(int rows, int columns) : base(rows, columns)
        {
            var i = 0;
            Populate((r, c, v) => new BigInteger(i++));
        }

        public List<object> GetAllHexes()
        {
            return Hexes;
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
    public class BasicHexGridCollectionTests
    {
        [TestMethod]
        public void TestSize()
        {
            var hgc = new HexGridCollectionAdapter(3,3);

            var hexes = hgc.GetAllHexes();

            Assert.AreEqual(hexes.Count, 3 + 2 + 3);
        }

        [TestMethod]
        public void TestListOffset()
        {
            var hgc = new HexGridCollectionAdapter(3 ,3);

            Assert.AreEqual(hgc.GetListOffset(0, 0), 0);
            Assert.AreEqual(hgc.GetListOffset(1, 0), 1);
            Assert.AreEqual(hgc.GetListOffset(2, 0), 2);
            // Assert.AreEqual(map.GetListOffset(0, 1), 3); // invalid coordinate
            Assert.AreEqual(hgc.GetListOffset(1, 1), 3);
            Assert.AreEqual(hgc.GetListOffset(2, 1), 4);
            Assert.AreEqual(hgc.GetListOffset(0, 2), 5);
            Assert.AreEqual(hgc.GetListOffset(1, 2), 6);
            Assert.AreEqual(hgc.GetListOffset(2, 2), 7);
        }

        [TestMethod]
        public void TestGetCoordsAreValid()
        {
            var hgc = new HexGridCollectionAdapter(3, 3);

            Assert.IsTrue(hgc.GetCoordsAreValid(0, 0));
            Assert.IsTrue(hgc.GetCoordsAreValid(1, 0));
            Assert.IsTrue(hgc.GetCoordsAreValid(2, 0));
            Assert.IsTrue(hgc.GetCoordsAreValid(1, 1));
            Assert.IsTrue(hgc.GetCoordsAreValid(2, 1));
            Assert.IsTrue(hgc.GetCoordsAreValid(0, 2));
            Assert.IsTrue(hgc.GetCoordsAreValid(1, 2));
            Assert.IsTrue(hgc.GetCoordsAreValid(2, 2));

            // 0 row has no elements in odd columns
            Assert.IsFalse(hgc.GetCoordsAreValid(0, 1));

            Assert.IsFalse(hgc.GetCoordsAreValid(-1, 0));
            Assert.IsFalse(hgc.GetCoordsAreValid(0, -1));

            Assert.IsFalse(hgc.GetCoordsAreValid(3, 0));
            Assert.IsFalse(hgc.GetCoordsAreValid(3, 2));

            Assert.IsFalse(hgc.GetCoordsAreValid(3, 0));
            Assert.IsFalse(hgc.GetCoordsAreValid(3, 1));
            Assert.IsFalse(hgc.GetCoordsAreValid(3, 2));
            Assert.IsFalse(hgc.GetCoordsAreValid(3, 3));
        }

        [TestMethod]
        public void TestIndexing()
        {
            var hgc = new HexGridCollectionAdapter(3,3);

            var allHexes = hgc.GetAllHexes();

            // this assumes that hexagons are generated column by column
            var i = 0;
            Assert.AreEqual(allHexes[i++], hgc.GetHex(0, 0));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(1, 0));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(2, 0));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(1, 1));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(2, 1));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(0, 2));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(1, 2));
            Assert.AreEqual(allHexes[i], hgc.GetHex(2, 2));

            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(0, 1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(-1, 0));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(0, -1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(3, 0));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(3, 2));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(0, 3));
        }

        [TestMethod]
        public void TestDirection()
        {
            var hgc = new HexGridCollectionAdapter(6, 6);

            var coords = new HexCoords() {Row = 4, Column = 3};

            coords = hgc.GetNeighbourCoords(coords, TileDirection.North);
            Assert.AreEqual(5, coords.Row);
            Assert.AreEqual(3, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, TileDirection.SouthEast);
            Assert.AreEqual(4, coords.Row);
            Assert.AreEqual(4, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, TileDirection.South);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(4, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, TileDirection.SouthWest);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(3, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, TileDirection.NorthWest);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(2, coords.Column);
            
            coords = hgc.GetNeighbourCoords(coords, TileDirection.North);
            Assert.AreEqual(4, coords.Row);
            Assert.AreEqual(2, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, TileDirection.NorthEast);
            Assert.AreEqual(5, coords.Row);
            Assert.AreEqual(3, coords.Column);

        }


        [TestMethod]
        public void TestAllNeighbours()
        {
            var map = new HexGridCollectionAdapter(8,8);

            var center = new HexCoords {Row = 4, Column = 3, Valid = true};

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(6, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetHex(5, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(4, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(3, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(3, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(3, 2)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(4, 2)));
        }

        [TestMethod]
        public void TestCornerNeighbours()
        {
            var map = new HexGridCollectionAdapter(6, 6);

            var corner = new HexCoords {Row = 0, Column = 0, Valid = true};

            var neighbours = map.GetNeighbours(corner);

            Assert.AreEqual(2, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetHex(1, 0)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(1, 1)));
        }

        [TestMethod]
        public void TestRadius2Neighbours()
        {
            var map = new HexGridCollectionAdapter(10, 10);

            var center = new HexCoords {Row = 5, Column = 5, Valid = true};

            var neighbours = map.GetNeighbours(center, 2);

            Assert.AreEqual(12, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetHex(7, 5)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(6, 6)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(6, 7)));

            Assert.IsTrue(neighbours.Contains(map.GetHex(5, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(4, 7)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(3, 6)));
            
            Assert.IsTrue(neighbours.Contains(map.GetHex(3, 5)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(3, 4)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(4, 3)));
            
            Assert.IsTrue(neighbours.Contains(map.GetHex(5, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(6, 3)));
            Assert.IsTrue(neighbours.Contains(map.GetHex(6, 4)));
        }

        [TestMethod]
        public void TestNoNeighbours()
        {
            var map = new HexGridCollectionAdapter(1, 1);

            var center = new HexCoords {Row = 0, Column = 0, Valid = true};

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(0, neighbours.Count);
        }
    }
}
