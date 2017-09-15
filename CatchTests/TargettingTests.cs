using System;
using Catch.Towers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    [TestClass]
    public class TargettingTests
    {
        private const float Pi = (float)Math.PI;
        private const float Eighth = 2 * Pi / 8;
        private const float Sixth = 2 * Pi / 6;
        private const float Clockwise = -1.0f;
        private const float AntiClockwise = 1.0f;

        private readonly float[] _clockwiseEighth = new[]
        {
            0 * Eighth * Clockwise,
            1 * Eighth * Clockwise,
            2 * Eighth * Clockwise,
            3 * Eighth * Clockwise,
            4 * Eighth * Clockwise,
            5 * Eighth * Clockwise,
            6 * Eighth * Clockwise,
            7 * Eighth * Clockwise,
        };

        private readonly float[] _clockwiseSixth = new[]
        {
            0 * Sixth * Clockwise,
            1 * Sixth * Clockwise,
            2 * Sixth * Clockwise,
            3 * Sixth * Clockwise,
            4 * Sixth * Clockwise,
            5 * Sixth * Clockwise,
        };

        private readonly float[] _antiClockwiseEighth = new[]
        {
            0 * Eighth * AntiClockwise,
            1 * Eighth * AntiClockwise,
            2 * Eighth * AntiClockwise,
            3 * Eighth * AntiClockwise,
            4 * Eighth * AntiClockwise,
            5 * Eighth * AntiClockwise,
            6 * Eighth * AntiClockwise,
            7 * Eighth * AntiClockwise,
        };

        private readonly float[] _antiClockwiseSixth = new[]
        {
            0 * Sixth * AntiClockwise,
            1 * Sixth * AntiClockwise,
            2 * Sixth * AntiClockwise,
            3 * Sixth * AntiClockwise,
            4 * Sixth * AntiClockwise,
            5 * Sixth * AntiClockwise,
        };

        [TestMethod]
        public void TestShortestRotationFromZeroEighth()
        {
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseEighth[0]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseEighth[1]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseEighth[2]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseEighth[3]), Clockwise, 0.01f);
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseEighth[4]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseEighth[5]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseEighth[6]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseEighth[7]), AntiClockwise, 0.01f);

            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseEighth[0]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseEighth[1]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseEighth[2]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseEighth[3]), AntiClockwise, 0.01f);
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseEighth[4]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseEighth[5]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseEighth[6]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseEighth[7]), Clockwise, 0.01f);
        }

        [TestMethod]
        public void TestShortestRotationFromZeroSixth()
        {
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseSixth[0]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseSixth[1]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseSixth[2]), Clockwise, 0.01f);
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseSixth[3]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseSixth[4]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _clockwiseSixth[5]), AntiClockwise, 0.01f);

            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseSixth[0]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseSixth[1]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseSixth[2]), AntiClockwise, 0.01f);
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseSixth[3]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseSixth[4]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(0.0f, _antiClockwiseSixth[5]), Clockwise, 0.01f);
        }

        [TestMethod]
        public void TestShortestRotationFromPiEighth()
        {
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseEighth[0]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseEighth[1]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseEighth[2]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseEighth[3]), AntiClockwise, 0.01f);
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseEighth[4]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseEighth[5]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseEighth[6]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseEighth[7]), Clockwise, 0.01f);

            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseEighth[0]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseEighth[1]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseEighth[2]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseEighth[3]), Clockwise, 0.01f);
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseEighth[4]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseEighth[5]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseEighth[6]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseEighth[7]), AntiClockwise, 0.01f);
        }

        [TestMethod]
        public void TestShortestRotationFromPiSixth()
        {
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseSixth[0]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseSixth[1]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseSixth[2]), AntiClockwise, 0.01f);
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseSixth[3]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseSixth[4]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _clockwiseSixth[5]), Clockwise, 0.01f);

            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseSixth[0]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseSixth[1]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseSixth[2]), Clockwise, 0.01f);
            // Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseSixth[3]), Clockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseSixth[4]), AntiClockwise, 0.01f);
            Assert.AreEqual(TargettingBase.ShortestRotationDirection(Pi, _antiClockwiseSixth[5]), AntiClockwise, 0.01f);
        }
    }
}
