using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchLibrary.Heap
{
    public class MinHeap<TP, TV> where TP : IComparable
    {
        private const int DefaultHeapSize = 16;

        private TP[] _heapPriorities;
        private TV[] _heapValues;
        private int _count;

        public MinHeap()
        {
            _heapPriorities = new TP[DefaultHeapSize];
            _heapValues = new TV[DefaultHeapSize];
            _count = 0;
        }

        public void Add(TP priority, TV value)
        {
            if (_count == _heapPriorities.Length)
                Double();

            // add new value to end heap
            var cur = _count;
            _heapPriorities[cur] = priority;
            _heapValues[cur] = value;

            // fixup heap property
            while (cur > 0)
            {
                var parent = ParentIndex(cur);

                // if current node is smaller than parent, then move current node closer to root
                if (_heapPriorities[cur].CompareTo(_heapPriorities[parent]) < 0)
                {
                    Swap(cur, parent);
                    cur = parent;
                }
                else
                {
                    break;
                }
            }

            // indicate updated size of heap
            _count += 1;
        }

        private int ParentIndex(int childIndex) => (childIndex + 1) / 2 - 1;

        private int LeftChildIndex(int parentIndex) => (parentIndex + 1) * 2 - 1;

        private int RightChildIndex(int parentIndex) => (parentIndex + 1) * 2;

        private void Swap(int cur, int parent)
        {
            var tp = _heapPriorities[cur];
            _heapPriorities[cur] = _heapPriorities[parent];
            _heapPriorities[parent] = tp;

            var tv = _heapValues[cur];
            _heapValues[cur] = _heapValues[parent];
            _heapValues[parent] = tv;
        }

        public int Count() => _count;

        public bool IsEmpty() => _count == 0;

        public TV Extract(out TP priority)
        {
            if (_count == 0)
                throw new IndexOutOfRangeException();

            // extract top of heap, and replace with last element
            var val = _heapValues[0];
            priority = _heapPriorities[0];
            var cur = _count--;  // post-decrement
            Swap(0, cur);

            // fixup heap property
            while (true)
            {
                var left = LeftChildIndex(cur);
                var right = RightChildIndex(cur);
                var smallest = cur;

                if (left < _count && _heapPriorities[left].CompareTo(_heapPriorities[smallest]) < 0)
                    smallest = left;
                if (right < _count && _heapPriorities[right].CompareTo(_heapPriorities[smallest]) < 0)
                    smallest = right;

                if (smallest != cur)
                    Swap(cur, smallest);
                else
                    break;
            }
            
            return val;
        }

        public TV Peek()
        {
            if (_count == 0)
                throw new IndexOutOfRangeException();

            return _heapValues[0];
        }

        public TP PeekPriority()
        {
            if (_count == 0)
                throw new IndexOutOfRangeException();

            return _heapPriorities[0];
        }

        private void Double()
        {
            var newHeapPriorities = new TP[_heapPriorities.Length * 2];
            var newHeapValues = new TV[_heapValues.Length * 2];

            _heapPriorities.CopyTo(newHeapPriorities, 0);
            _heapValues.CopyTo(newHeapValues, 0);

            _heapPriorities = newHeapPriorities;
            _heapValues = newHeapValues;
        }
    }
}
