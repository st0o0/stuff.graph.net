using System.Diagnostics;
using stuff.graph.algorithms.net;
using stuff.graph.net;
using stuff.graph.serializable.net;
using stuff.graph.sortingbarrier.net;
using stuff.graph.wcc.net;
using Xunit.Abstractions;

namespace stuff.graph.tests;

public class SortingBarrierTests
{
    private readonly ITestOutputHelper _output;

    public SortingBarrierTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private static Graph BuildSimpleGraph()
    {
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));

        // Nodes
        var n1 = builder.CreateNode(1, 0, 0, 0);
        var n2 = builder.CreateNode(2, 0, 0, 0);
        var n3 = builder.CreateNode(3, 0, 0, 0);
        var n4 = builder.CreateNode(4, 0, 0, 0);

        // Edges
        var e12 = builder.CreateDirectedEdge(100, 1, 2, 1, EdgeDirection.OneWay);
        var e23 = builder.CreateDirectedEdge(101, 2, 3, 2, EdgeDirection.OneWay);
        var e13 = builder.CreateDirectedEdge(102, 1, 3, 10, EdgeDirection.OneWay);
        var e34 = builder.CreateDirectedEdge(103, 3, 4, 3, EdgeDirection.OneWay);


        n1.AddOutgoing(e12.Id);
        n1.AddOutgoing(e13.Id);

        n2.AddIncoming(e12.Id);
        n2.AddOutgoing(e23.Id);

        n3.AddIncoming(e23.Id);
        n3.AddIncoming(e13.Id);
        n3.AddOutgoing(e34.Id);

        n4.AddIncoming(e34.Id);

        return builder.CreateGraph();
    }

    [Fact]
    public void ShortestPath_Should_FindDirectConnection()
    {
        var graph = BuildSimpleGraph();
        var settings = new SortingBarrierSettings(1, 4);
        var algo = SortingBarrier.Create(new SortingBarrierConfig(graph, settings));

        var path = algo.GetShortestPath(new SearchArgs(graph.GetNode(1), graph.GetNode(2)));

        Assert.NotNull(path);
        Assert.Equal(1, path.SourceNodeId);
        Assert.Equal(2, path.TargetNodeId);
        Assert.Equal([1, 2], path.Nodes.Select(n => n.Id));
    }

    [Fact]
    public void ShortestPath_Should_ChooseLowerCostRoute()
    {
        var graph = BuildSimpleGraph();
        var settings = new SortingBarrierSettings(1, 4);
        var algo = SortingBarrier.Create(new SortingBarrierConfig(graph, settings));

        var path = algo.GetShortestPath(new SearchArgs(graph.GetNode(1), graph.GetNode(3)));

        Assert.NotNull(path);
        Assert.Equal([1, 2, 3], path.Nodes.Select(n => n.Id));
    }

    [Fact]
    public void ShortestPath_Should_ReturnEmpty_WhenNoPathExists()
    {
        var graph = BuildSimpleGraph();
        var settings = new SortingBarrierSettings(1, 4);
        var algo = SortingBarrier.Create(new SortingBarrierConfig(graph, settings));

        var path = algo.GetShortestPath(new SearchArgs(graph.GetNode(4), graph.GetNode(1)));

        Assert.NotNull(path);
        Assert.Empty(path.Nodes);
    }

    [Fact]
    public void ShortestPath_Should_ReachEndNode()
    {
        var graph = BuildSimpleGraph();
        var settings = new SortingBarrierSettings(1, 4);
        var algo = SortingBarrier.Create(new SortingBarrierConfig(graph, settings));

        var path = algo.GetShortestPath(new SearchArgs(graph.GetNode(1), graph.GetNode(4)));

        Assert.NotNull(path);
        Assert.Equal([1, 2, 3, 4], path.Nodes.Select(n => n.Id));
    }

    [Fact]
    public void Test_WCC_SortingBarrier_On_CustomMap()
    {
        var json = "./newmap.json";
        var jsonGraph = AstarTests.MapLoader.Load(json);
        var graph = jsonGraph.To();

        var algo = WeaklyConnectedComponents.Create(new WCCConfig(graph));
        var watch = new Stopwatch();
        watch.Start();
        var result = algo.Find();
        _output.WriteLine($"WCC: {watch.ElapsedMilliseconds}ms");
        Assert.Equal(2, result.Length);
        var biggestGraph = result.OrderByDescending(x => x.Edges.Count + x.Nodes.Count).First();
        var source = biggestGraph.Nodes.Min(x => x.Key);
        var target = biggestGraph.Nodes.Max(x => x.Key);
        var a = SortingBarrier.Create(new SortingBarrierConfig(biggestGraph, new SortingBarrierSettings(1, 4)));
        watch.Restart();
        var path = a.GetShortestPath(new SearchArgs(biggestGraph.GetNode(source), biggestGraph.GetNode(target)));
        watch.Stop();
        _output.WriteLine($"sortingbarrier: {watch.ElapsedMilliseconds}ms");
        Assert.NotNull(path);
        Assert.NotEmpty(path.Nodes);
        Assert.Equal(96, path.Nodes.Length);
    }
}