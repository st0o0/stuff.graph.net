using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using stuff.graph.algorithms.net;
using stuff.graph.astar.net;
using stuff.graph.di.net;
using stuff.graph.dijkstra.net;
using stuff.graph.net;
using stuff.graph.sortingbarrier.net;
using Path = stuff.graph.algorithms.net.Path;

namespace stuff.graph.benchmark.net;

[RPlotExporter]
[DisassemblyDiagnoser]
[MemoryDiagnoser]
[WarmupCount(2)]
[IterationCount(8)]
[InvocationCount(1)]
public class BigGraphShortestPathBenchmarks
{
    public enum GraphTopology
    {
        Grid,
        Random
    }

    [Params(60000)] public int NodeCount;

    [Params(GraphTopology.Grid, GraphTopology.Random)] public GraphTopology Topology;

    [Params(3)] public int ExtraEdgesPerNode;

    private ServiceProvider _root = null!;
    private Graph _graph = null!;
    private IDijkstra _dijkstra = null!;
    private IAStar _aStar = null!;
    private ISortingBarrier _sortingBarrier = null!;

    private SearchArgs _args;

    [GlobalSetup]
    public void Setup()
    {
        _root = new ServiceCollection()
            .AddGraphStuff(_ => BuildGraph())
            .BuildServiceProvider();

        using var scope = _root.CreateScope();
        var sp = scope.ServiceProvider;

        _graph = sp.GetRequiredService<IGraphProvider>().GetGraph();
        _dijkstra = sp.GetRequiredService<IDijkstra>();
        _aStar = sp.GetRequiredService<IAStar>();
        _sortingBarrier = sp.GetRequiredService<ISortingBarrier>();

        var targetId = _graph.Nodes.Keys.Max();
        _args = new SearchArgs(_graph.GetNode(1), _graph.GetNode(targetId));
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _root.Dispose();
    }

    // ----------------- Benchmarks -----------------

    [Benchmark(Baseline = true)]
    public Path Dijkstra()
        => _dijkstra.GetShortestPath(_args);

    [Benchmark]
    public Path AStar()
        => _aStar.GetShortestPath(_args);

    [Benchmark]
    public Path SortingBarrier()
        => _sortingBarrier.GetShortestPath(_args);

    // ----------------- Graph Builder -----------------

    private Graph BuildGraph()
        => Topology switch
        {
            GraphTopology.Grid => BuildGridGraphFor60K(),
            GraphTopology.Random => BuildRandomGraphWithBackbone(NodeCount, ExtraEdgesPerNode),
            _ => throw new NotSupportedException()
        };

    private static Graph BuildGridGraphFor60K()
    {
        const int w = 245, h = 245;
        var g = new Graph { Id = Guid.NewGuid() };

        long nid = 1;
        var nodes = new Node[w, h];
        for (var x = 0; x < w; x++)
        {
            for (var y = 0; y < h; y++)
            {
                var n = Node.Create(nid++, x, y, 0);
                g.Nodes[n.Id] = n;
                nodes[x, y] = n;
            }
        }

        long eid = 1;
        for (var x = 0; x < w; x++)
        {
            for (var y = 0; y < h; y++)
            {
                var cur = nodes[x, y];

                if (x + 1 < w)
                {
                    var right = nodes[x + 1, y];
                    var e = new DirectedEdge
                    {
                        Id = eid++,
                        StartNodeId = cur.Id,
                        EndNodeId = right.Id,
                        RoutingCost = 1,
                        Direction = EdgeDirection.TwoWay
                    };
                    g.Edges[e.Id] = e;
                    cur.AddOutgoing(e.Id);
                    right.AddIncoming(e.Id);
                    right.AddOutgoing(e.Id);
                    cur.AddIncoming(e.Id);
                }

                if (y + 1 < h)
                {
                    var up = nodes[x, y + 1];
                    var e = new DirectedEdge
                    {
                        Id = eid++,
                        StartNodeId = cur.Id,
                        EndNodeId = up.Id,
                        RoutingCost = 1,
                        Direction = EdgeDirection.TwoWay
                    };
                    g.Edges[e.Id] = e;

                    cur.AddOutgoing(e.Id);
                    up.AddIncoming(e.Id);
                    up.AddOutgoing(e.Id);
                    cur.AddIncoming(e.Id);
                }
            }
        }

        return g;
    }

    private static Graph BuildRandomGraphWithBackbone(int nodeCount, int extraEdgesPerNode)
    {
        var g = new Graph { Id = Guid.NewGuid() };
        var rnd = new Random(42);

        for (var i = 1; i <= nodeCount; i++)
        {
            var n = Node.Create(i, rnd.Next(0, 1000), rnd.Next(0, 1000), 0);
            g.Nodes[n.Id] = n;
        }

        var eid = 1;
        for (var i = 1; i < nodeCount; i++)
        {
            var e = new DirectedEdge
            {
                Id = eid++,
                StartNodeId = i,
                EndNodeId = i + 1,
                RoutingCost = 1,
                Direction = EdgeDirection.TwoWay
            };
            g.Edges[e.Id] = e;

            g.Nodes[i].AddOutgoing(e.Id);
            g.Nodes[i + 1].AddIncoming(e.Id);

            g.Nodes[i + 1].AddOutgoing(e.Id);
            g.Nodes[i].AddIncoming(e.Id);
        }

        var nodeIds = g.Nodes.Keys.ToArray();
        foreach (var id in nodeIds)
        {
            for (var k = 0; k < extraEdgesPerNode; k++)
            {
                var target = nodeIds[rnd.Next(nodeIds.Length)];
                if (target == id) continue;

                var dir = RandomDirection(rnd);
                var cost = (uint)rnd.Next(1, 20);

                var e = new DirectedEdge
                {
                    Id = eid++,
                    StartNodeId = id,
                    EndNodeId = target,
                    RoutingCost = cost,
                    Direction = dir
                };
                g.Edges[e.Id] = e;

                switch (dir)
                {
                    case EdgeDirection.Blocked:
                    case EdgeDirection.OneWay:
                        g.Nodes[id].AddOutgoing(e.Id);
                        g.Nodes[target].AddIncoming(e.Id);
                        break;
                    case EdgeDirection.TwoWay:
                        g.Nodes[id].AddOutgoing(e.Id);
                        g.Nodes[target].AddIncoming(e.Id);

                        g.Nodes[target].AddOutgoing(e.Id);
                        g.Nodes[id].AddIncoming(e.Id);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dir));
                }
            }
        }

        return g;

        static EdgeDirection RandomDirection(Random rnd)
        {
            // 70% TwoWay, 25% OneWay, 5% Blocked
            var p = rnd.NextDouble();
            return p switch
            {
                < 0.05 => EdgeDirection.Blocked,
                < 0.30 => EdgeDirection.OneWay,
                _ => EdgeDirection.TwoWay
            };
        }
    }
}