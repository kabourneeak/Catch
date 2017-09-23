using System;
using System.Collections;
using System.Collections.Generic;
using Catch.Base;

namespace Catch.Services
{
    public class VersionedCollection<T> : IVersionedCollection<T>
    {
        private readonly ICollection<T> _collection;

        public VersionedCollection(ICollection<T> collection)
        {
            _collection = collection
                          ?? throw new ArgumentNullException(nameof(collection));
        }

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _collection).GetEnumerator();

        public void Add(T item)
        {
            _collection.Add(item);
            ++Version;
        }

        public void Clear()
        {
            _collection.Clear();
            ++Version;
        }

        public bool Contains(T item) => _collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            var wasRemoved = _collection.Remove(item);

            if (wasRemoved)
                ++Version;

            return wasRemoved;
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;

        public int Version { get; private set; }
    }
}