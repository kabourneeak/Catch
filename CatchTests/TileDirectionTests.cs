using CatchLibrary.HexGrid;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    [TestClass]
    public class TileDirectionTests
    {
        [TestMethod]
        public void TestShortestRotation()
        {
            const float clockwise = -1.0f;
            const float anticlockwise = 1.0f;

            Assert.AreEqual(HexDirectionExtensions.ShortestRotationDirection(HexDirection.North, HexDirection.NorthWest), anticlockwise, 0.01f);
            Assert.AreEqual(HexDirectionExtensions.ShortestRotationDirection(HexDirection.North, HexDirection.SouthWest), anticlockwise, 0.01f);
            Assert.AreEqual(HexDirectionExtensions.ShortestRotationDirection(HexDirection.North, HexDirection.SouthEast), clockwise, 0.01f);
            Assert.AreEqual(HexDirectionExtensions.ShortestRotationDirection(HexDirection.North, HexDirection.NorthEast), clockwise, 0.01f);

            Assert.AreEqual(HexDirectionExtensions.ShortestRotationDirection(HexDirection.NorthWest, HexDirection.SouthWest), anticlockwise, 0.01f);
            Assert.AreEqual(HexDirectionExtensions.ShortestRotationDirection(HexDirection.NorthWest, HexDirection.South), anticlockwise, 0.01f);
            Assert.AreEqual(HexDirectionExtensions.ShortestRotationDirection(HexDirection.NorthWest, HexDirection.North), clockwise, 0.01f);
            Assert.AreEqual(HexDirectionExtensions.ShortestRotationDirection(HexDirection.NorthWest, HexDirection.NorthEast), clockwise, 0.01f);
        }
    }
}
