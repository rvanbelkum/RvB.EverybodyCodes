using RvB.Linq;
using System.Numerics;

namespace RvB.EC.Shared;

public static class StringRangeExtensions {
    extension<T>(Iterable<Split, StringRange> iterator) where T : INumber<T> {
        public T[] ToNumberArray() {
            return iterator.Select(n => T.Parse(n, null)).ToArray();
        }

        public List<T> ToNumberList() {
            return iterator.Select(n => T.Parse(n, null)).ToList();
        }
    }

    extension(Iterable<Split, StringRange> iterator) {
        public (int, int, int) GetCountAndMinMaxLength() {
            var minLength = int.MaxValue;
            var maxLength = int.MinValue;
            var count = 0;
            foreach (var item in iterator) {
                count += 1;
                minLength = Math.Min(minLength, item.Length);
                maxLength = Math.Max(maxLength, item.Length);
            }
            return (count, minLength, maxLength);
        }
    }
}
