#pragma warning disable IDE0005
global using System.Diagnostics;
global using RvB.Puzzles.Shared;
global using static RvB.Puzzles.Shared.Display;
global using static System.Console;
#pragma warning restore IDE0005

using System.Reflection;
using RvB.CommandLine;

namespace RvB.EverybodyCodes;

internal class Program {
    private static HandleOutput s_handleOutput = HandleOutput.Save;

    private static void HandleResult(object? result, int year, string quest, string part, string assetsPath, List<string> validationErrors) {
        if (result is not null) {
            var filename = Path.Combine(assetsPath, "Results.txt");
            Dictionary<string, string> results = [];
            var key = $"{quest}-{part}";

            if (s_handleOutput == HandleOutput.Save) {
                if (File.Exists(filename)) {
                    var lines = File.ReadAllLines(filename);
                    results = lines.ToDictionary(l => l.Split('\t')[0], l => l.Split('\t')[1]);
                }
                if (result is ValueTuple<long, string> valueArray) {
                    result = valueArray.Item1;
                }
                results[key] = result.ToString()!;
                File.WriteAllLines(filename, results.Select(kv => $"{kv.Key}\t{kv.Value}"));
            } else if (s_handleOutput == HandleOutput.Validate) {
                if (File.Exists(filename)) {
                    var lines = File.ReadAllLines(filename);
                    results = lines.ToDictionary(l => l.Split('\t')[0], l => l.Split('\t')[1]);
                    if (results.TryGetValue(key, out var value)) {
                        if (result is ValueTuple<long, string> valueArray) {
                            result = valueArray.Item1;
                        }
                        if (value != result.ToString()) {
                            validationErrors.Add($"[{year} {key}] Validation failed. Expected: [{value}], result: [{result}]");
                            WriteLine($"Validation failed. Expected: [{value}], result: [{result}]", ConsoleColor.Red);
                        }
                    } else {
                        validationErrors.Add($"[{year} {key}] Validation failed: Known good result not found.");
                    }
                } else {
                    var error = $"[{year}] Validation failed: Cannot find results file.";
                    if (!validationErrors.Contains(error)) {
                        validationErrors.Add(error);
                    }
                }
            }
        }
    }

    static long?[] RunQuest(int year, Type questClass, bool microSeconds, int benchmark, List<string> validationErrors) {
        var timings = new long?[3];
        if (questClass is not null) {
            var assetsPath = Path.GetFullPath($@"..\..\..\..\..\RvB.Puzzles.Assets\EC{year}\");
            var inputFileName = Path.Combine(assetsPath, $"{questClass.Name}.txt");

            PuzzleInput input;
            for (var p = 1; p <= 3; p += 1) {
                var inputFileNamePart = Path.Combine(assetsPath, $"{questClass.Name}.{p}.txt");
                if (File.Exists(inputFileNamePart)) {
                    input = new PuzzleInput(File.ReadAllText(inputFileNamePart));
                } else if (File.Exists(inputFileName)) {
                    input = new PuzzleInput(File.ReadAllText(inputFileName));
                } else {
                    input = new PuzzleInput(string.Empty);
                }
                var @base = (Quest?)Activator.CreateInstance(questClass);
                if (@base is null) {
                    WriteLine($"Cannot create {questClass.Name} in {year}.", ConsoleColor.Red);
                    return [0, 0, 0];
                }
                WriteLine($"{year} - {questClass.Name}");
                var m = @base.GetType().GetMethod($"DoPart{p}");
                var x = m?.Invoke(@base, [input, microSeconds, benchmark]);
                object? result = null;
                if (x != null) {
                    (result, timings[p-1]) = ((object, long?))x;
                }
                HandleResult(result, year, questClass.Name, $"Part {p}", assetsPath, validationErrors);
            }
        }
        return timings;
    }

    class CmdLine : DefaultOptions {
        [Option('y', "Years", MaxValues = 100, Delimiter = ',')]
        public int[]? Years { get; set; }

        [Option('q', "Quests", MaxValues = 25, Delimiter = ',')]
        public int[]? Quests { get; set; }

        [Option('l', "Latest", IsSwitch = true)]
        public bool Latest { get; set; } = false;

        [Option('w', "Validate", IsSwitch = true)]
        public bool Validate { get; set; } = false;

        [Option('b', "Benchmark")]
        public int Benchmark { get; set; } = 1;

        [Option('u', "Micro", IsSwitch = true)]
        public bool Microseconds { get; set; } = false;
    }

    static void Main(string[] args) {
        var parseResult = Parser<CmdLine>.Parse(args, o => {
            o.HandleDefaultOptions = true;
            o.HandleErrors = true;
        });
        if (parseResult.HasErrors) {
            Console.WriteLine("Finished!");
            Console.ReadKey();
            return;
        }
        var parsedArgs = parseResult.Parameters;

        if (parsedArgs.Years is null || parsedArgs.Years.Length == 0) {
            var prefix = $"RvB.EC";
            var postfix = ".dll";

            parsedArgs.Years = Directory.EnumerateFiles(Environment.CurrentDirectory)
                .Select(f => Path.GetFileName(f))
                .Where(f => f.Length == prefix.Length + 4 + postfix.Length && f.StartsWith(prefix) && f.EndsWith(postfix) && int.TryParse(f[prefix.Length..^postfix.Length], out _))
                .Select(f => int.Parse(f[prefix.Length..^postfix.Length]))
                .ToArray();
        }
        List<string> validationErrors = [];
        if (parsedArgs.Validate) {
            s_handleOutput = HandleOutput.Validate;
        }
        List<(int Year, string Quest, long?[] Time)> timings = [];
        foreach (var year in parsedArgs.Years) {
            var assembly = Assembly.Load($"RvB.EC{year}");
            var questClasses = assembly.GetTypes()
                .Where(t => t.Name.Length >= 5 && t.Name.StartsWith("Quest"))
                .OrderBy(t => int.Parse(t.Name.AsSpan()[5..]));

            if (parsedArgs.Quests is not null && parsedArgs.Quests.Length > 0) {
                foreach (var quest in parsedArgs.Quests) {
                    var questClass = questClasses.FirstOrDefault(d => d.Name == $"Quest{quest:00}");
                    if (questClass is not null) {
                        timings.Add((year, questClass.Name, RunQuest(year, questClass, parsedArgs.Microseconds, parsedArgs.Benchmark, validationErrors)));
                    }
                }
            } else if (parsedArgs.Latest) {
                var questClass = questClasses.LastOrDefault();
                if (questClass is not null) {
                    timings.Add((year, questClass.Name, RunQuest(year, questClass, parsedArgs.Microseconds, parsedArgs.Benchmark, validationErrors)));
                }
            } else {
                questClasses.ForEach(d => timings.Add((year, d.Name, RunQuest(year, d, parsedArgs.Microseconds, parsedArgs.Benchmark, validationErrors))));
            }
        }
        if (timings.Count > 1) {
            var file = File.CreateText(@"Timings.txt");
            file.WriteLine("Year\tQuest\tPart1\tPart2");
            foreach (var t in timings) {
                file.WriteLine($"{t.Year}\t{t.Quest[3..]}\t{t.Time[0]:n0}\t{t.Time[1]:n0}\t{t.Time[2]:n0}");
            }
            file.Close();
            WriteLine($"Timing results in {(parsedArgs.Microseconds ? "μs" : "ms")}");
            var tableOptions = new ASCIITableOptions([
                    new() { Title = "Year", Alignment = Alignment.Center, Color = ConsoleColor.Gray },
                    new() { Title = "Quest", Alignment = Alignment.Center, Color = ConsoleColor.Gray },
                    new() { Title = "Part 1", Alignment = Alignment.Right, Color = ConsoleColor.White },
                    new() { Title = "Part 2", Alignment = Alignment.Right, Color = ConsoleColor.White },
                    new() { Title = "Part 3", Alignment = Alignment.Right, Color = ConsoleColor.White }
                    ]) {
                BorderColor = ConsoleColor.Cyan,
                BorderStyle = BorderStyle.DoubleOutside,
                HorizontalPadding = 2,
                DefaultHeaderAlignment = Alignment.Center
            };
            ASCIITable.DrawTable(
                tableOptions,
                timings.Select(t => new string[] { $"{t.Year}", $"{t.Quest[5..]}", $"{t.Time[0]:n0}", $"{t.Time[1]:n0}", $"{t.Time[2]:n0}" })
                );
            if (validationErrors.Count > 0) {
                WriteLine("Validation Errors");
                foreach (var error in validationErrors) {
                    WriteLine(error, ConsoleColor.Red);
                }
                WriteLine();
            }
        }
        WriteLine("Program finished. Press [enter] to quit.");
        Console.ReadLine();
    }
}
