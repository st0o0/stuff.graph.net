using stuff.graph.algorithms.net;
using stuff.graph.net;
using stuff.graph.sortingbarrier.net;
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
        var e12 = builder.CreateEdge(100,1 ,2, 1);
        var e23 = builder.CreateEdge(101,2 ,3, 2);
        var e13 = builder.CreateEdge(102,1 ,3, 10);
        var e34 = builder.CreateEdge(103,3 ,4, 3);


        // Connect Nodes
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
}