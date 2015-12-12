using Catch.Base;
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

            Assert.AreEqual(TileDirectionExtensions.ShortestRotationDirection(TileDirection.North, TileDirection.NorthWest), anticlockwise, 0.01f);
            Assert.AreEqual(TileDirectionExtensions.ShortestRotationDirection(TileDirection.North, TileDirection.SouthWest), anticlockwise, 0.01f);
            Assert.AreEqual(TileDirectionExtensions.ShortestRotationDirection(TileDirection.North, TileDirection.SouthEast), clockwise, 0.01f);
            Assert.AreEqual(TileDirectionExtensions.ShortestRotationDirection(TileDirection.North, TileDirection.NorthEast), clockwise, 0.01f);

            Assert.AreEqual(TileDirectionExtensions.ShortestRotationDirection(TileDirection.NorthWest, TileDirection.SouthWest), anticlockwise, 0.01f);
            Assert.AreEqual(TileDirectionExtensions.ShortestRotationDirection(TileDirection.NorthWest, TileDirection.South), anticlockwise, 0.01f);
            Assert.AreEqual(TileDirectionExtensions.ShortestRotationDirection(TileDirection.NorthWest, TileDirection.North), clockwise, 0.01f);
            Assert.AreEqual(TileDirectionExtensions.ShortestRotationDirection(TileDirection.NorthWest, TileDirection.NorthEast), clockwise, 0.01f);
        }
    }
}
