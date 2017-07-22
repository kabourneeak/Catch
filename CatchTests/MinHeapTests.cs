using System;
using CatchLibrary.Heap;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CatchTests
{
    [TestClass]
    public class MinHeapTests
    {
        private MinHeap<float, object> CreateTestSubject()
        {
            return new MinHeap<float, object>();
        }
            
        [TestMethod]
        public void ConstructorTest()
        {
            var subject = CreateTestSubject();
        }

        [TestMethod]        
        public void EmptyHeapCannotPeek()
        {
            var subject = CreateTestSubject();
            Assert.ThrowsException<IndexOutOfRangeException>(() => subject.Peek());
        }

        [TestMethod]
        public void EmptyHeapCannotExtractTest()
        {
            var subject = CreateTestSubject();
            Assert.ThrowsException<IndexOutOfRangeException>(() => subject.Extract(out _));
        }

        [TestMethod]
        public void EmptyHeapReportsEmptyTest()
        {
            var subject = CreateTestSubject();

            Assert.IsTrue(subject.IsEmpty);
            Assert.AreEqual(0, subject.Count);
        }

        [TestMethod]
        public void InsertInOrderTest()
        {
            var subject = CreateTestSubject();

            var firstObject = new object();
            subject.Add(1.0f, firstObject);
            Assert.IsTrue(object.ReferenceEquals(firstObject, subject.Peek()));

            for(int i = 2; i < 100; ++i)
            {
                subject.Add(i, new object());

                // first object stays at the front
                Assert.IsTrue(object.ReferenceEquals(firstObject, subject.Peek()));
            }
        }

        [TestMethod]
        public void InsertReverseOrderTest()
        {
            var subject = CreateTestSubject();

            for (int i = 100; i > 0; --i)
            {
                var obj = new object();
                subject.Add(i, obj);

                // most recently added object goes to the front
                Assert.IsTrue(object.ReferenceEquals(obj, subject.Peek()));
            }
        }

        [TestMethod]
        public void ExtractTest()
        {
            var subject = CreateTestSubject();

            // insert
            for (int i = 100; i > 0; --i)
            {
                var obj = new object();
                subject.Add(i, obj);
            }

            // extract
            var last = 0.0;
            while (!subject.IsEmpty)
            {
                subject.Extract(out var cur);

                // it's a min heap, so each item should be larger than the preceeding one
                Assert.IsTrue(cur > last);
                last = cur;
            }
        }

        [TestMethod]
        public void IncreaseKeyTest()
        {
            // assemble
            var subject = CreateTestSubject();

            var firstObject = new object();
            subject.Add(1.0f, firstObject);
            var secondObject = new object();
            subject.Add(2.0f, secondObject);

            // act
            subject.Increase(3.0f);

            // assert
            Assert.IsTrue(object.ReferenceEquals(secondObject, subject.Extract(out _)));
            Assert.IsTrue(object.ReferenceEquals(firstObject, subject.Extract(out _)));
            Assert.IsTrue(subject.IsEmpty);
        }
    }
}
