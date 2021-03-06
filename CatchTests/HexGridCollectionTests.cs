﻿using System;
using System.Linq;
using System.Numerics;
using CatchLibrary.HexGrid;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    internal class HexGridCollectionAdapter : HexGridCollection<object>
    {
        public HexGridCollectionAdapter(int rows, int columns) : base(rows, columns)
        {
            var i = 0;
            Populate((hc, v) => new BigInteger(i++));
        }

        public new HexCoords GetNeighbourCoords(HexCoords coords, HexDirection direction)
        {
            return base.GetNeighbourCoords(coords, direction);
        }

        public new int GetOffset(HexCoords hc)
        {
            return base.GetOffset(hc);
        }
    }

    [TestClass]
    public class BasicHexGridCollectionTests
    {
        [TestMethod]
        public void TestSize()
        {
            var hgc = new HexGridCollectionAdapter(3,3);

            var hexes = hgc.ToList();

            Assert.AreEqual(hexes.Count, 3 * 3);
        }

        [TestMethod]
        public void TestListOffset()
        {
            var hgc = new HexGridCollectionAdapter(3 ,3);

            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(0, 0)), 0);
            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(1, 0)), 1);
            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(2, 0)), 2);

            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(0, 1)), 3);
            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(1, 1)), 4);
            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(2, 1)), 5);

            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(0, 2)), 6);
            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(1, 2)), 7);
            Assert.AreEqual(hgc.GetOffset(HexCoords.CreateFromOffset(2, 2)), 8);
        }

        [TestMethod]
        public void TestGetCoordsAreValid()
        {
            var hgc = new HexGridCollectionAdapter(3, 3);

            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(0, 0)));
            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(1, 0)));
            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(2, 0)));
            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(0, 1)));
            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(1, 1)));
            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(2, 1)));
            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(0, 2)));
            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(1, 2)));
            Assert.IsTrue(hgc.HasHex(HexCoords.CreateFromOffset(2, 2)));

            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(-1, 0)));
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(0, -1)));

            // everything in row 3 is out of bounds
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(3, 0)));
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(3, 1)));
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(3, 2)));
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(3, 3)));

            // everything in column 3 is out of bounds
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(0, 3)));
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(1, 3)));
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(2, 3)));
            Assert.IsFalse(hgc.HasHex(HexCoords.CreateFromOffset(3, 3)));
        }

        [TestMethod]
        public void TestIndexing()
        {
            var hgc = new HexGridCollectionAdapter(3, 3);

            var allHexes = hgc.ToList();

            // this assumes that hexagons are generated column by column
            var i = 0;
            Assert.AreEqual(allHexes[i++], hgc.GetHex(HexCoords.CreateFromOffset(0, 0)));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(HexCoords.CreateFromOffset(1, 0)));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(HexCoords.CreateFromOffset(2, 0)));

            Assert.AreEqual(allHexes[i++], hgc.GetHex(HexCoords.CreateFromOffset(0, 1)));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(HexCoords.CreateFromOffset(1, 1)));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(HexCoords.CreateFromOffset(2, 1)));

            Assert.AreEqual(allHexes[i++], hgc.GetHex(HexCoords.CreateFromOffset(0, 2)));
            Assert.AreEqual(allHexes[i++], hgc.GetHex(HexCoords.CreateFromOffset(1, 2)));
            Assert.AreEqual(allHexes[i], hgc.GetHex(HexCoords.CreateFromOffset(2, 2)));

            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(HexCoords.CreateFromOffset(-1, 0)));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(HexCoords.CreateFromOffset(0, -1)));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(HexCoords.CreateFromOffset(3, 0)));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(HexCoords.CreateFromOffset(3, 2)));
            Assert.ThrowsException<IndexOutOfRangeException>(() => hgc.GetHex(HexCoords.CreateFromOffset(0, 3)));
        }

        [TestMethod]
        public void TestDirection()
        {
            var hgc = new HexGridCollectionAdapter(6, 6);

            var coords = HexCoords.CreateFromOffset(4,3);

            coords = hgc.GetNeighbourCoords(coords, HexDirection.North);
            Assert.AreEqual(5, coords.Row);
            Assert.AreEqual(3, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, HexDirection.SouthEast);
            Assert.AreEqual(4, coords.Row);
            Assert.AreEqual(4, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, HexDirection.South);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(4, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, HexDirection.SouthWest);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(3, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, HexDirection.NorthWest);
            Assert.AreEqual(3, coords.Row);
            Assert.AreEqual(2, coords.Column);
            
            coords = hgc.GetNeighbourCoords(coords, HexDirection.North);
            Assert.AreEqual(4, coords.Row);
            Assert.AreEqual(2, coords.Column);

            coords = hgc.GetNeighbourCoords(coords, HexDirection.NorthEast);
            Assert.AreEqual(5, coords.Row);
            Assert.AreEqual(3, coords.Column);

        }


        [TestMethod]
        public void TestAllNeighbours()
        {
            var map = new HexGridCollectionAdapter(8,8);

            var center = HexCoords.CreateFromOffset(4, 3);

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(6, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(5, 3))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(4, 4))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(3, 4))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(3, 3))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(3, 2))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(4, 2))));
        }

        [TestMethod]
        public void TestCornerNeighbours()
        {
            var map = new HexGridCollectionAdapter(6, 6);

            var corner = HexCoords.CreateFromOffset(0, 0);

            var neighbours = map.GetNeighbours(corner);

            Assert.AreEqual(3, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(1, 0))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(1, 1))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(0, 1))));
        }

        [TestMethod]
        public void TestRadius2Neighbours()
        {
            var map = new HexGridCollectionAdapter(10, 10);

            var center = HexCoords.CreateFromOffset(5, 5);

            var neighbours = map.GetNeighbours(center, 2);

            Assert.AreEqual(12, neighbours.Count);

            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(7, 5))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(6, 6))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(6, 7))));

            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(5, 7))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(4, 7))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(3, 6))));
            
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(3, 5))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(3, 4))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(4, 3))));
            
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(5, 3))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(6, 3))));
            Assert.IsTrue(neighbours.Contains(map.GetHex(HexCoords.CreateFromOffset(6, 4))));
        }

        [TestMethod]
        public void TestNoNeighbours()
        {
            var map = new HexGridCollectionAdapter(1, 1);

            var center = HexCoords.CreateFromOffset(0, 0);

            var neighbours = map.GetNeighbours(center);

            Assert.AreEqual(0, neighbours.Count);
        }
    }
}
