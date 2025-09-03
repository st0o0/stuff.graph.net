using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.Extensions.DependencyInjection;
using stuff.graph.algorithms.net;
using stuff.graph.astar.net;
using stuff.graph.di.net;
using stuff.graph.dijkstra.net;
using stuff.graph.net;
using stuff.graph.sortingbarrier.net;

namespace stuff.graph.benchmark.net;

[RPlotExporter]
[DisassemblyDiagnoser]
[MemoryDiagnoser]
[Config(typeof(Config))]
[SimpleJob(RunStrategy.ColdStart, launchCount: 200)]
public class ShortestPathBenchmarks
{
    private Graph _graph = null!;
    private IDijkstra _dijkstra = null!;
    private IAStar _astar = null!;
    private ISortingBarrier _sortingBarrier = null!;
    private ISearchArgs _args = null!;

    [GlobalSetup]
    public void Setup()
    {
        _graph = BuildGraph(200, 400);
        var services = new ServiceCollection()
            .AddGraphStuff(_ => _graph)
            .BuildServiceProvider();

        var provider = services.CreateScope().ServiceProvider;
        _dijkstra = provider.GetRequiredService<IDijkstra>();
        _astar = provider.GetRequiredService<IAStar>();
        _sortingBarrier = provider.GetRequiredService<ISortingBarrier>();
        _args = new SearchArgs(_graph.GetNode(1), _graph.GetNode(150));
    }

    private static Graph BuildGraph(int nodeCount, int edgeCount)
    {
        var builder = GraphBuilder.Create();
        var rnd = new Random(42);

        // Nodes
        for (var i = 1; i <= nodeCount; i++)
        {
            _ = builder.CreateNode(i, rnd.Next(0, 100), rnd.Next(0, 100), 0);
        }

        // Edges
        long eid = 1;
        var nodeIds = builder.Nodes.Keys.ToArray();
        for (var i = 0; i < edgeCount; i++)
        {
            var start = nodeIds[rnd.Next(nodeIds.Length)];
            var end = nodeIds[rnd.Next(nodeIds.Length)];
            if (start == end) continue;

            var cost = (uint)rnd.Next(1, 20);
            var edge = builder.CreateDirectedEdge(eid++, start, end, cost, EdgeDirection.OneWay);
            builder.Nodes[start].AddOutgoing(edge.Id);
            builder.Nodes[end].AddIncoming(edge.Id);
        }

        return builder.CreateGraph();
    }

    [Benchmark]
    public void Dijkstra()
    {
        _ = _dijkstra.GetShortestPath(_args);
    }

    [Benchmark]
    public void AStar()
    {
        _ = _astar.GetShortestPath(_args);
    }

    [Benchmark]
    public void SortingBarrierSSSP()
    {
        _ = _sortingBarrier.GetShortestPath(_args);
    }
}