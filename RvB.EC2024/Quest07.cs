using System.Text;

namespace RvB.EC2024;

public class Quest07 : Quest {
    public override object? Part1(PuzzleInput input) {
        var result = new Dictionary<string, int>();
        foreach (var line in input.GetLines()) {
            var (first, rest) = line.Split(':');
            var essence = 10;
            var sum = 0;
            var actions = rest.Split(',').Select(r => r[0]).ToArray();
            for (var step = 0; step < 10; step += 1) {
                essence = actions[step % actions.Length] switch {
                    '-' => Math.Max(0, essence - 1),
                    '+' => essence + 1,
                    _ => essence
                };
                sum += essence;
            }
            result[first.ToString()] = sum;
        }
        return string.Join("", result.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
    }

    public override object? Part2(PuzzleInput input) {
        var (actionsDef, trackLayout) = input.AsSections();
        var grid = GridFactory.CreateCharGrid(trackLayout);
        var sb = new StringBuilder();
        GridPos last = (0, 0);
        GridPos next = (1, 0);
        while (next != (0, 0)) {
            sb.Append(grid[next]);
            (next, last) = (grid.GetCardinalNeighbors(next).First(p => p != last && grid[p] != ' '), next);
        }
        sb.Append('='); // instead of the 'S', we can make it '=' so we don't need to check for both.
        var track = sb.ToString();

        var result = new Dictionary<string, int>();
        foreach (var line in actionsDef.GetLines()) {
            var (first, rest) = line.Split(':');
            var essence = 10;
            var sum = 0;
            var actions = rest.Split(',').Select(r => r[0]).ToArray();
            var actionIndex = 0;
            for (var loop = 0; loop < 10; loop += 1) {
                for (var step = 0; step < track.Length; step += 1) {
                    var path = track[step];
                    var action = path == '=' ? actions[actionIndex] : path;
                    if (++actionIndex == actions.Length)
                        actionIndex = 0;
                    essence = action switch {
                        '-' => Math.Max(0, essence - 1),
                        '+' => essence + 1,
                        _ => essence
                    };
                    sum += essence;
                }
            }
            result[first.ToString()] = sum;
        }
        return string.Join("", result.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
    }

    public override object? Part3(PuzzleInput input) {
        var (actionsDef, trackLayout) = input.AsSections();
        var grid = GridFactory.CreateCharGrid(trackLayout);
        var sb = new StringBuilder();
        GridPos last = (0, 0);
        GridPos next = (1, 0);
        while (next != (0, 0)) {
            sb.Append(grid[next]);
            (next, last) = (grid.GetCardinalNeighbors(next).First(p => p != last && grid[p] != ' '), next);
        }
        sb.Append('='); // instead of the 'S', we can make it '=' so we don't need to check for both.
        var track = sb.ToString();

        var (_, rest) = actionsDef.AsStringRange().Split(':');
        var actions = rest.Split(',').Select(r => r[0]).ToArray();

        // We don't need to do all 2024 loops. After maxLoops we're back in the starting position.
        var maxLoops = actions.Length / MathEx.GCD(track.Length, actions.Length);

        var rivalScore = GetActionPlanScore(actions);

        var winCount = 0;
        var count = 0;
        foreach (var perm in "+++++---===".EnumerateUniquePermutations()) {
            var score = GetActionPlanScore(perm);
            if (score > rivalScore) {
                winCount += 1;
            }
            count += 1;
        }
        return winCount;

        long GetActionPlanScore(ReadOnlySpan<char> actions) {
            var actionIndex = 0;
            var essence = 10;
            var sum = 0L;
            for (var loop = 0; loop < maxLoops; loop += 1) {
                foreach (var path in track) {
                    var action = path == '=' ? actions[actionIndex] : path;
                    if (++actionIndex == actions.Length)
                        actionIndex = 0;
                    essence = action switch {
                        '-' => Math.Max(0, essence - 1),
                        '+' => essence + 1,
                        _ => essence
                    };
                    sum += essence;
                }
            }
            return sum;
        }
    }
}
