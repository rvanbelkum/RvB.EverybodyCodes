using System.Collections;

namespace RvB.EC.Shared;

internal static class CollectionHelpers {
    public static IReadOnlyCollection<T> ReifyCollection<T>(IEnumerable<T> source) {
        ArgumentNullException.ThrowIfNull(source);

        if (source is IReadOnlyCollection<T> result)
            return result;
        if (source is ICollection<T> collection)
            return new CollectionWrapper<T>(collection);
        if (source is ICollection nongenericCollection)
            return new NongenericCollectionWrapper<T>(nongenericCollection);

        return new List<T>(source);
    }

    private sealed class NongenericCollectionWrapper<T>(ICollection collection) : IReadOnlyCollection<T> {
        private readonly ICollection _collection = collection ?? throw new ArgumentNullException(nameof(collection));

        public int Count => _collection.Count;

        public IEnumerator<T> GetEnumerator() {
            foreach (T item in _collection)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class CollectionWrapper<T>(ICollection<T> collection) : IReadOnlyCollection<T> {
        private readonly ICollection<T> _collection = collection ?? throw new ArgumentNullException(nameof(collection));

        public int Count => _collection.Count;

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
