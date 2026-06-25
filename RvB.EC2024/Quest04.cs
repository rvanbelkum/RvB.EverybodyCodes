namespace RvB.EC2024;

public class Quest04 : Quest {
    public override object? Part1(PuzzleInput input) {
        var nails = input.GetLines().ToNumberArray<int>();
        var min = nails.Min();
        return nails.Sum(n => n - min);
    }

    public override object? Part2(PuzzleInput input) {
        return Part1(input);
    }

    public override object? Part3(PuzzleInput input) {
        var nails = input.GetLines().ToNumberArray<long>();
        Array.Sort(nails);
        var median = nails[nails.Length / 2];
        return nails.Sum(n => Math.Abs(n - median));
    }
}
