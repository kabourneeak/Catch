using System.Collections.Generic;

namespace Catch.Base
{
    /// <summary>
    /// A collection which increments a version number each time there is a change to membership of the collection.
    /// The version is _not_ incremented for changes inside objects stored in the collection
    /// </summary>
    /// <typeparam name="T">The type to store in the collection</typeparam>
    public interface IVersionedCollection<T> : ICollection<T>
    {
        int Version { get; }    
    }
}