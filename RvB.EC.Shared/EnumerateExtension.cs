namespace RvB.EC.Shared;

public static class EnumerateExtension {
    public static void ForEach<T>(this IEnumerable<T> elements, Action<T> action) {
        foreach (var element in elements)
            action(element);
    }
}
