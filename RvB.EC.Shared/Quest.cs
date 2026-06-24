using static RvB.EC.Shared.Display;
using static System.Console;

namespace RvB.EC.Shared;

public enum HandleOutput {
    None,
    Save,
    Validate
}

public abstract class Quest {
    public HandleOutput HandleOutput { get; set; } = HandleOutput.Validate;

    public (object? Result, long? TimeInMs) DoPart1(PuzzleInput input, bool microSeconds, int benchmark)
        => DoPart("Part 1", input, Part1, microSeconds, benchmark);

    public (object? Result, long? TimeInMs) DoPart2(PuzzleInput input, bool microSeconds, int benchmark)
        => DoPart("Part 2", input, Part2, microSeconds, benchmark);

    public (object? Result, long? TimeInMs) DoPart3(PuzzleInput input, bool microSeconds, int benchmark)
        => DoPart("Part 3", input, Part3, microSeconds, benchmark);

    private static (object? Result, long? TimeInMs) DoPart(string name, PuzzleInput input, Func<PuzzleInput, object?> part, bool microSeconds, int benchmark) {
        WriteLine($"\t{name}", ConsoleColor.Yellow);
        object? result = null;
        var timing = Benchmark.BenchmarkTime(() => result = part(input), benchmark, Benchmark.Measure.Min);
        if (result is null) {
            WriteLine("\tNo result\r\n", ConsoleColor.Magenta);
            return (null, null);
        }
        if (!microSeconds) {
            timing /= 1000;
        }
        var metric = microSeconds ? "μs" : "ms";
        WriteLine($"\tresult = {result}");
        WriteLine($"\tFinished in {timing}{metric}\r\n", ConsoleColor.Green);
        if (result is string strResult) {
            result = strResult.Replace(Environment.NewLine, "_");
        }
        return (result, timing);
    }


    public abstract object? Part1(PuzzleInput input);
    public abstract object? Part2(PuzzleInput input);
    public abstract object? Part3(PuzzleInput input);
}
