using System;

namespace CatchLibrary.Heap
{
    /// <summary>
    /// A basic array-backed min heap, which grows as needed when items are added
    /// </summary>
    /// <typeparam name="TP">The type for the priorities</typeparam>
    /// <typeparam name="TV">The type for the values</typeparam>
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

        public int Count => _count;

        public bool IsEmpty => _count == 0;

        /// <summary>
        /// Add an item to the queue with the given priority
        /// </summary>
        public void Add(TP priority, TV value)
        {
            if (_count == _heapPriorities.Length)
                GrowHeap();

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

        /// <summary>
        /// Examine the next item on the queue (the item with minimum priority) without removing it
        /// </summary>
        /// <returns>The next item in the queue</returns>
        public TV Peek()
        {
            if (_count == 0)
                throw new IndexOutOfRangeException();

            return _heapValues[0];
        }

        /// <returns>The priority of the next item on the queue</returns>
        public TP PeekPriority()
        {
            if (_count == 0)
                throw new IndexOutOfRangeException();

            return _heapPriorities[0];
        }

        /// <summary>
        /// Dequeue the next item on the queue (the item with minimum priority)
        /// </summary>        
        /// <param name="priority">The priority the removed item had on the queue</param>
        /// <returns>The next item on the queue</returns>
        public TV Extract(out TP priority)
        {
            if (_count == 0)
                throw new IndexOutOfRangeException();

            // extract top of heap, and replace with last element
            var val = _heapValues[0];
            priority = _heapPriorities[0];
            _count -= 1;
            Swap(0, _count);

            // fixup heap property from root
            BubbleDown(0);

            return val;
        }

        /// <summary>
        /// Increases the priority of the next item on the heap, the general use of which is to
        /// reschedule it for later in the queue.
        /// </summary>
        /// <param name="newPriority"></param>
        public void Increase(TP newPriority)
        {
            if (_count == 0)
                throw new IndexOutOfRangeException();

            _heapPriorities[0] = newPriority;
            BubbleDown(0);
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

        private void BubbleDown(int index)
        {
            while (true)
            {
                var left = LeftChildIndex(index);
                var right = RightChildIndex(index);
                var smallest = index;

                if (left < _count && _heapPriorities[left].CompareTo(_heapPriorities[smallest]) < 0)
                    smallest = left;
                if (right < _count && _heapPriorities[right].CompareTo(_heapPriorities[smallest]) < 0)
                    smallest = right;

                if (smallest != index)
                {
                    Swap(index, smallest);
                    index = smallest;
                }
                else
                {
                    break;
                }
            }
        }

        private void GrowHeap()
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
