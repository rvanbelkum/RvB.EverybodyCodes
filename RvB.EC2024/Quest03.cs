namespace RvB.EC2024;

public class Quest03 : Quest {
    public override object? Part1(PuzzleInput input) {
        var grid = GridFactory.Create(input.AsStringRange(), c => c == '#' ? 0 : -1);
        var count = 0;
        do {
            var canBeChanged = grid
                .Where(c => c.Value == 0 || (c.Value > 0 && grid.GetCardinalNeighbors(c.Pos).All(n => grid[n] == c.Value)))
                .Select(c => c.Pos)
                .ToArray();
            if (canBeChanged.Length == 0)
                break;
            foreach (var cell in canBeChanged) {
                grid[cell] += 1;
            }
            count += canBeChanged.Length;
        } while (true);
        return count;
    }

    public override object? Part2(PuzzleInput input) {
        return Part1(input);
    }

    public override object? Part3(PuzzleInput input) {
        var grid = GridFactory.Create(input.AsStringRange(), c => c == '#' ? 0 : -1);
        var count = 0;
        do {
            var canBeChanged = grid
                .Where(c => {
                    if (c.Value <= 0)
                        return c.Value == 0;
                    var neighbors = grid.GetAllNeighbors(c.Pos);
                    return neighbors.Count == 8 && grid.GetAllNeighbors(c.Pos).All(n => grid[n] == c.Value);
                })
                .Select(c => c.Pos)
                .ToArray();
            if (canBeChanged.Length == 0)
                break;
            foreach (var cell in canBeChanged) {
                grid[cell] += 1;
            }
            count += canBeChanged.Length;
        } while (true);
        return count;
    }
}
