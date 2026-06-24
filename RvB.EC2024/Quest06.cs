using RvB.EC.Shared;
using RvB.Graphs;
using RvB.Linq;

namespace RvB.EC2024;

public class Quest06 : Quest {
    public override object? Part1(PuzzleInput input) {
        var graph = CreateGraph(input);
        var dists = BuildDistanceMap(graph, "RR", (p, n) => p + n);
        return dists.First(kv => kv.Value.Count == 1).Value[0];
    }

    public override object? Part2(PuzzleInput input) {
        var graph = CreateGraph(input);
        var dists = BuildDistanceMap(graph, "RR", (p, n) => p + n[0]);
        return dists.First(kv => kv.Value.Count == 1).Value[0];
    }

    public override object? Part3(PuzzleInput input) {
        var graph = CreateGraph(input);
        var dists = BuildDistanceMap(graph, "RR", (p, n) => p + n[0]);
        return dists.First(kv => kv.Value.Count == 1).Value[0];
    }

    private static Dictionary<int, List<string>> BuildDistanceMap(Graph<string> graph, string start, Func<string, string, string> buildPath) {
        var distMap = new Dictionary<int, List<string>>();
        var toDo = new Queue<(GraphVertex<string>, string, int)>();
        var visited = new HashSet<string>();
        toDo.Enqueue((graph[start], buildPath(string.Empty, start), 0));
        while (toDo.TryDequeue(out var item)) {
            var (node, path, dist) = item;
            if (!visited.Add(node)) {
                continue;
            }
            foreach (var next in node.Connections) {
                if (next == "@") {
                    distMap.AddOrUpdate(dist + 1, buildPath(path, next.Vertex));
                    continue;
                }
                toDo.Enqueue((next, buildPath(path, next.Vertex), dist + 1));
            }
        }
        return distMap;
    }

    private static Graph<string> CreateGraph(PuzzleInput input) {
        var graph = new Graph<string>(GraphType.Directed);
        foreach (var line in input.GetLines()) {
            foreach (var vertex in line.SplitAny(":,")) {
                graph.AddVertex(vertex.ToString());
            }
        }
        foreach (var line in input.GetLines()) {
            var (from, rest) = line.Split(':');
            var fromStr = from.ToString();
            foreach (var to in rest.Split(',')) {
                var toStr = to.ToString();
                if (fromStr != toStr) {
                    graph.AddEdge(fromStr, toStr);
                }
            }
        }
        return graph;
    }
}
