using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RvB.EC.Shared;

public static class DictionaryExtension {
    extension<TKey, TValue>(Dictionary<TKey, TValue> dict) where TKey : notnull where TValue : notnull, INumber<TValue> {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddOrUpdate(TKey key, TValue value) {
            ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
            if (exists) {
                existingValue = existingValue! + value;
            } else {
                existingValue = value;
            }
        }
    }

    extension<TKey, TValue>(Dictionary<TKey, TValue> dict) where TKey : notnull where TValue : notnull {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue GetOrAdd(TKey key, Func<TValue> create) {
            ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
            if (!exists) {
                var currentValue = create();
                existingValue = currentValue;
            }
            return existingValue!;
        }
    }

    extension<TKey, TValue, TCollection>(Dictionary<TKey, TCollection> dict) where TKey : notnull where TCollection : ICollection<TValue>, new() {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddOrUpdate(TKey key, TValue value) {
            ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
            if (exists) {
                existingValue!.Add(value);
            } else {
                existingValue = [value];
            }
        }
    }
}
