using System.Diagnostics;
using stuff.graph.dijkstra.net;
using stuff.graph.net;
using stuff.graph.serializable.net;
using stuff.graph.wcc.net;
using Xunit.Abstractions;
using stuff.graph.algorithms.net;
using static stuff.graph.tests.AstarTests;

namespace stuff.graph.tests;

public class DijkstraTests
{
    private readonly ITestOutputHelper _output;
    public DijkstraTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private static Graph SetupGraph()
    {
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));

        var nodeA = builder.CreateNode(1, 0, 0, 0);
        var nodeB = builder.CreateNode(2, 1, 0, 0);
        var nodeC = builder.CreateNode(3, 2, 0, 0);
        var nodeD = builder.CreateNode(4, 1, 1, 0);

        var edgeAB = builder.CreateEdge(1, 1, 2, 1);
        var edgeBC = builder.CreateEdge(2, 2, 3, 2);
        var edgeAD = builder.CreateEdge(3, 1, 4, 4);
        var edgeDC = builder.CreateEdge(4, 4, 3, 1);
        nodeA.AddOutgoing(edgeAB.Id);
        nodeB.AddIncoming(edgeAB.Id);

        nodeB.AddOutgoing(edgeBC.Id);
        nodeC.AddIncoming(edgeBC.Id);

        nodeA.AddOutgoing(edgeAD.Id);
        nodeD.AddIncoming(edgeAD.Id);

        nodeD.AddOutgoing(edgeDC.Id);
        nodeC.AddIncoming(edgeDC.Id);
        return builder.CreateGraph();
    }

    [Fact]
    public void TestShortestPath_AtoC()
    {
        // Arrange
        var graph = SetupGraph();
        var pathfinder = Dijkstra.Create(new DijkstraConfig(graph));

        // Act
        var shortestPath = pathfinder.GetShortestPath(new SearchArgs(graph.GetNode(1), graph.GetNode(3)));

        // Assert
        var expectedPath = new List<long> { 1, 2, 3 };
        Assert.NotNull(shortestPath);
        Assert.Equal(expectedPath, shortestPath.Nodes.Select(x => x.Id));
    }

    [Fact]
    public void TestShortestPath_AtoD()
    {
        // Arrange
        var graph = SetupGraph();
        var pathfinder = Dijkstra.Create(new DijkstraConfig(graph));

        // Act
        var shortestPath = pathfinder.GetShortestPath(new SearchArgs(graph.GetNode(1), graph.GetNode(4)));

        // Assert
        var expectedPath = new List<long> { 1, 4 };
        Assert.NotNull(shortestPath);
        Assert.Equal(expectedPath, shortestPath.Nodes.Select(x => x.Id));
    }

    [Fact]
    public void TestNoPath()
    {
        // Arrange
        var graph = SetupGraph();
        var pathfinder = Dijkstra.Create(new DijkstraConfig(graph));

        graph.Edges.Remove(2);
        graph.Edges.Remove(4);

        // Act
        var path = pathfinder.GetShortestPath(new SearchArgs(graph.GetNode(1), graph.GetNode(3)));

        // Assert
        Assert.NotNull(path);
        Assert.Empty(path.Nodes);
    }

    [Fact]
    public void TestSameNode()
    {
        // Arrange
        var graph = SetupGraph();
        var pathfinder = Dijkstra.Create(new DijkstraConfig(graph));

        // Act
        var shortestPath = pathfinder.GetShortestPath(new SearchArgs(graph.GetNode(1), graph.GetNode(1)));

        // Assert
        var expectedPath = new List<long> { 1 };
        Assert.NotNull(shortestPath);
        Assert.Equal(expectedPath, shortestPath.Nodes.Select(x => x.Id));
    }

    [Fact]
    public void Test_WCC_Dijkstra_On_CustomMap()
    {
        var json = "./newmap.json";
        var jsonGraph = MapLoader.Load(json);
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
        var a = Dijkstra.Create(new DijkstraConfig(biggestGraph));
        watch.Restart();
        var path = a.GetShortestPath(new SearchArgs(biggestGraph.GetNode(source), biggestGraph.GetNode(target)));
        watch.Stop();
        _output.WriteLine($"dijkstra: {watch.ElapsedMilliseconds}ms");
        Assert.NotNull(path);
        Assert.NotEmpty(path.Nodes);
        Assert.Equal(96, path.Nodes.Length);
    }
}
