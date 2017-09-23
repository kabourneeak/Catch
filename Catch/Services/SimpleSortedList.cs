using System.Collections;
using System.Collections.Generic;

namespace Catch.Services
{
    /// <summary>
    /// A very naive sorting list which attempts to sort the entire list using the supplied comparer
    /// whenever anything is added.  C# uses insertion sort for lists of 15 elements or less, which is very
    /// fast for mostly sorted lists
    /// </summary>
    public class SimpleSortedList<T> : IList<T>
    {
        private readonly IComparer<T> _comparer;
        private readonly List<T> _list;

        public SimpleSortedList(IComparer<T> comparer)
        {
            _comparer = comparer;
            _list = new List<T>();
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _list).GetEnumerator();

        public void Add(T item)
        {
            _list.Add(item);
            _list.Sort(_comparer);
        }

        public void Clear() => _list.Clear();

        public bool Contains(T item) => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public bool Remove(T item) => _list.Remove(item);

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public int IndexOf(T item) => _list.IndexOf(item);

        public void Insert(int index, T item) => _list.Insert(index, item);

        public void RemoveAt(int index) => _list.RemoveAt(index);

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }
    }
}
