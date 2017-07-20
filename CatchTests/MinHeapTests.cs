using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void EmptyHeapCannotExtract()
        {
            var subject = CreateTestSubject();
            Assert.ThrowsException<IndexOutOfRangeException>(() => subject.Extract(out _));
        }

        [TestMethod]
        public void EmptyHeapReportsEmpty()
        {
            var subject = CreateTestSubject();

            Assert.IsTrue(subject.IsEmpty());
            Assert.AreEqual(0, subject.Count());
        }

        [TestMethod]
        public void InsertInOrder()
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
        public void InsertReverseOrder()
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
    }
}
