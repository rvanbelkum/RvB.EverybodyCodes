using RvB.EC.Shared;
using RvB.Linq;

namespace RvB.EC2024;

public class Quest02 : Quest {
    public override object? Part1(PuzzleInput input) {
        var (s1, s2) = input.AsSections();
        var words = s1.AsStringRange()[6..].Split(',').ToList();
        var text = s2.AsStringRange();
        var count = 0;
        foreach (var word in words) {
            var idx = text.IndexOf(word);
            while (idx != -1) {
                count += 1;
                idx = text.IndexOf(word, idx + 1);
            }
        }
        return count;
    }

    public override object? Part2(PuzzleInput input) {
        var (s1, s2) = input.AsSections();
        var words = s1.AsStringRange()[6..].Split(',').ToArray();
        words = words.Concat(words.Select(w => w.Reverse())).ToArray();

        var count = 0;
        foreach (var text in s2.GetLines()) {
            count += Count(text);
        }
        return count;

        int Count(StringRange text) {
            var used = new bool[text.Length];
            foreach (var word in words) {
                var idx = text.IndexOf(word);
                while (idx != -1) {
                    Array.Fill(used, true, idx, word.Length);
                    idx = text.IndexOf(word, idx + 1);
                }
            }
            return used.Count(u => u);
        }
    }

    public override object? Part3(PuzzleInput input) {
        var (s1, s2) = input.AsSections();
        var words = s1.AsStringRange()[6..].Split(',').ToArray();
        words = words.Concat(words.Select(w => w.Reverse())).ToArray();

        var rows = s2.GetLines().Select(l => l.ToString()).ToArray();
        var cols = Enumerable.Range(0, rows[0].Length).Select(i => string.Join("", rows.Select(t => t[i]))).ToArray();

        var symbols = new HashSet<(int, int)>();
        var used = new bool[rows.Length, rows[0].Length];

        for (var r = 0; r < rows.Length; r += 1) {
            foreach (var col in Count(rows[r], true)) {
                symbols.Add((col, r));
            }
        }
        for (var c = 0; c < cols.Length; c += 1) {
            foreach (var row in Count(cols[c], false)) {
                symbols.Add((c, row));
            }
        }
        return symbols.Count;

        IEnumerable<int> Count(string text, bool wrap) {
            foreach (var word in words) {
                var idx = text.IndexOf(word[0]);
                while (idx != -1 && (wrap || idx + word.Length <= text.Length)) {
                    if (word.Index().All(wi => text[(wi.Index + idx) % text.Length] == wi.Item)) {
                        foreach (var index in Enumerable.Range(0, word.Length)) {
                            yield return (index + idx) % text.Length;
                        }
                    }
                    idx = text.IndexOf(word[0], idx + 1);
                }
            }
        }
    }
}
