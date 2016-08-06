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
            var hc1 = new HexCoords() {Row = 0, Column = 0};
            var hc2 = new HexCoords() {Row = 0, Column = 0};
            var hc3 = new HexCoords() {Row = 0, Column = 1};
            var hc4 = new HexCoords() {Row = 1, Column = 0};

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
                    list.Add(new HexCoords() {Row = row, Column = col});

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
    }
}
