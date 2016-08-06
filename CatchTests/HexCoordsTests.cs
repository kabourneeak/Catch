using System.Collections.Generic;
using CatchLibrary.HexGrid;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    [TestClass]
    public class HexCoordsTests
    {
        [TestMethod]
        public void TestEquality()
        {
            var hc1 = HexCoords.CreateFromOffset(0, 0);
            var hc2 = HexCoords.CreateFromOffset(0, 0);
            var hc3 = HexCoords.CreateFromOffset(0, 1);
            var hc4 = HexCoords.CreateFromOffset(1, 0);

            Assert.AreNotEqual(hc1, null);
            Assert.AreNotEqual(hc1, 5);
            Assert.AreNotEqual(hc1, "string");

            Assert.AreEqual(hc1, hc1);
            Assert.AreEqual(hc1, hc2);
            Assert.AreEqual(hc2, hc1);

            Assert.AreNotEqual(hc1, hc3);
            Assert.AreNotEqual(hc1, hc4);
        }

        [TestMethod]
        public void TestPerformance()
        {
            const int rows = 100;
            const int cols = 100;
            const int searchRepetitions = 10;

            var list = new List<HexCoords>(rows * cols);
            var dict = new Dictionary<HexCoords, HexCoords>();

            // prep
            for (var row = 0; row < rows; ++row)
                for (var col = 0; col < cols; ++col)
                    list.Add(HexCoords.CreateFromOffset(row, col));

            // add
            var addWatch = System.Diagnostics.Stopwatch.StartNew();

            foreach (var hc in list)
            {
                dict.Add(hc, hc);
            }

            addWatch.Stop();
            System.Diagnostics.Debug.WriteLine($"Elapsed time for adding: {addWatch.ElapsedMilliseconds}ms");

            // search
            var searchWatch = System.Diagnostics.Stopwatch.StartNew();

            var hasKey = true;

            for (var i = 0; i < searchRepetitions; ++i)
                foreach (var hc in list)
                    hasKey = hasKey & dict.ContainsKey(hc);

            searchWatch.Stop();
            System.Diagnostics.Debug.WriteLine($"Elapsed time for searching: {searchWatch.ElapsedMilliseconds}ms");

            // pass test
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestAxialOffset()
        {
            // origin case
            var hc1 = HexCoords.CreateFromAxial(0, 0);
            Assert.AreEqual(0, hc1.Column);
            Assert.AreEqual(0, hc1.Row);

            // +ve cases
            var hc2 = HexCoords.CreateFromAxial(1, 1);
            Assert.AreEqual(1, hc2.Column);
            Assert.AreEqual(2, hc2.Row);

            var hc3 = HexCoords.CreateFromAxial(2, 1);
            Assert.AreEqual(2, hc3.Column);
            Assert.AreEqual(2, hc3.Row);

            var hc4 = HexCoords.CreateFromAxial(1, 2);
            Assert.AreEqual(1, hc4.Column);
            Assert.AreEqual(3, hc4.Row);

            // +ve, -ve cases
            var hc5 = HexCoords.CreateFromAxial(-1, 2);
            Assert.AreEqual(-1, hc5.Column);
            Assert.AreEqual(2, hc5.Row);

            var hc6 = HexCoords.CreateFromAxial(3, -1);
            Assert.AreEqual(3, hc6.Column);
            Assert.AreEqual(1, hc6.Row);

            // -ve cases
            var hc7 = HexCoords.CreateFromAxial(-1, -2);
            Assert.AreEqual(-1, hc7.Column);
            Assert.AreEqual(-2, hc7.Row);
        }
    }
}