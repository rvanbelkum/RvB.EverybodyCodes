using RvB.EC.Shared;
using RvB.Linq;

namespace RvB.EC2024;

public class Quest01 : Quest {
    private Dictionary<char, int> _potionMap = new() {
        ['A'] = 0, ['B'] = 1, ['C'] = 3, ['D'] = 5
    };

    public override object? Part1(PuzzleInput input) {
        return input.AsStringRange().Sum(c => _potionMap.TryGetValue(c, out var p) ? p : 0);
    }

    public override object? Part2(PuzzleInput input) {
        return CountByChuck(input, 2);
    }

    public override object? Part3(PuzzleInput input) {
        return CountByChuck(input, 3);
    }

    private int CountByChuck(StringRange text, int chunkSize) {
        return text
            .Chunk(chunkSize)
            .Sum(group => {
                var count = group.Count(c => _potionMap.ContainsKey(c));
                return group.Sum(countPotions);

                int countPotions(char g) => _potionMap.TryGetValue(g, out int p) ? p + count - 1 : 0;
            });
    }
}
