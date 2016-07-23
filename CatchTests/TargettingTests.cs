using System;
using Catch.Towers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    [TestClass]
    public class TargettingTests
    {
        [TestMethod]
        public void TestShortestRotation()
        {
            const float pi = (float) Math.PI;
            const float clockwise = -1.0f;
            const float anticlockwise = 1.0f;

            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, 1.0f), anticlockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, pi - 1.0f), anticlockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, pi + 1.0f), clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, 2 * pi - 1.0f), clockwise, 0.01f);

            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, -1.0f), clockwise, 0.01f);
        }
    }
}
