using System.Runtime.InteropServices;

namespace RvB.EC2024;

public class Quest05 : Quest {
    public override object? Part1(PuzzleInput input) {
        var dancers = ReadDancers(input);
        for (var round = 0; round < 10; round += 1) {
            MakeAMove(dancers, round);
        }
        return string.Join("", dancers.Select(d => d.First!.Value.ToString()));
    }

    public override object? Part2(PuzzleInput input) {
        var dancers = ReadDancers(input);
        var round = 0;
        var shoutCounts = new Dictionary<int, int>();
        int lastShout = 0;
        while (true) {
            MakeAMove(dancers, round);
            lastShout = dancers.Aggregate(0, (a, d) => 100 * a + d.First!.Value);
            ref var shoutCount = ref CollectionsMarshal.GetValueRefOrAddDefault(shoutCounts, lastShout, out _);
            round += 1;
            if (++shoutCount == 2024) {
                break;
            }
        }
        return (long)round * lastShout;
    }

    public override object? Part3(PuzzleInput input) {
        var dancers = ReadDancers(input);
        var states = new HashSet<ValueArray<int>>();
        var highestShout = 0L;
        for (var round = 0; ; round += 1) {
            MakeAMove(dancers, round);
            highestShout = Math.Max(highestShout, dancers.Aggregate(0L, (a, d) => 10000L * a + d.First!.Value));
            var state = new ValueArray<int>(dancers.SelectMany(d => d));
            if (!states.Add(state)) {
                break;
            }
        }
        return highestShout;
    }

    private static void MakeAMove(List<LinkedList<int>> dancers, int round) {
        var column = dancers[round % dancers.Count];
        var chestNumber = column.First!.Value;
        column.RemoveFirst();

        var nextColumn = dancers[(round + 1) % dancers.Count];
        var moves = (chestNumber - 1) % (2 * nextColumn.Count) + 1;
        if (moves <= nextColumn.Count) {
            var first = nextColumn.First!;
            while (--moves > 0) {
                first = first!.Next;
            }
            nextColumn.AddBefore(first!, chestNumber);
        } else {
            moves -= nextColumn.Count;
            var last = nextColumn.Last!;
            while (--moves > 0) {
                last = last!.Previous;
            }
            nextColumn.AddAfter(last!, chestNumber);
        }
    }

    private static List<LinkedList<int>> ReadDancers(PuzzleInput input) {
        List<LinkedList<int>> dancers = [];
        var e = input.GetLines();
        e.MoveNext();
        foreach (var n in e.Current.Split(' ').Select(i => int.Parse(i))) {
            dancers.Add(new LinkedList<int>([n]));
        }
        while (e.MoveNext()) {
            var c = 0;
            foreach (var n in e.Current.Split(' ').Select(i => int.Parse(i))) {
                dancers[c++].AddLast(n);
            }
        }
        return dancers;
    }
}
