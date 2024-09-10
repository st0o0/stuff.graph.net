using System.Diagnostics;
using stuff.graph.astar.net;
using stuff.graph.net;
using stuff.graph.serializable.net;
using stuff.graph.wcc.net;
using Xunit.Abstractions;

namespace stuff.graph.tests;

public partial class AstarTests
{
    private readonly ITestOutputHelper _output;

    public AstarTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void FindPath_ShouldReturnCorrectPath_WhenPathExists()
    {
        // Arrange;
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));
        var startNode = builder.CreateNode(1, 0, 0, 0);
        var middleNode = builder.CreateNode(2, 1, 1, 1);
        var endNode = builder.CreateNode(3, 2, 2, 2);

        var edge1 = builder.CreateEdge(1, startNode.Id, middleNode.Id);
        var edge2 = builder.CreateEdge(2, middleNode.Id, endNode.Id);
        var graph = builder.CreateGraph();

        var settings = new AStarSettings(Heuristic.Manhatten);
        var astar = AStar.Create(new AStarConfig(graph, settings));

        // Act
        var path = astar.GetShortestPath(new SearchPath(startNode, endNode));

        // Assert
        Assert.NotNull(path);
        Assert.Equal(3, path.Nodes.Length);
        Assert.Equal([startNode, middleNode, endNode], path.Nodes);
    }

    [Fact]
    public void FindPath_ShouldReturnNull_WhenNoPathExists()
    {
        // Arrange
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));
        var startNode = builder.CreateNode(1, 0, 0, 0);
        var isolatedNode = builder.CreateNode(2, 10, 10, 10);
        var graph = builder.CreateGraph();
        var settings = new AStarSettings(Heuristic.Manhatten);
        var astar = AStar.Create(new AStarConfig(graph, settings));

        // Act
        var path = astar.GetShortestPath(new SearchPath(startNode, isolatedNode));

        // Assert
        Assert.Null(path);
    }

    [Fact]
    public void FindPath_ShouldHandleSingleStepPath()
    {
        // Arrange
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));
        var startNode = builder.CreateNode(1, 0, 0, 0);
        var endNode = builder.CreateNode(2, 1, 1, 1);
        var edge = builder.CreateEdge(1, startNode.Id, endNode.Id);
        var graph = builder.CreateGraph();
        var settings = new AStarSettings(Heuristic.Manhatten);
        var astar = AStar.Create(new AStarConfig(graph, settings));

        // Act
        var path = astar.GetShortestPath(new SearchPath(startNode, endNode));

        // Assert
        Assert.NotNull(path);
        Assert.Equal(2, path.Nodes.Length);
        Assert.Equal([startNode, endNode], path.Nodes);
    }

    [Fact]
    public void Test_WCC_AStar_On_CustomMap()
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
        var a = AStar.Create(new AStarConfig(biggestGraph, new AStarSettings(Heuristic.Manhatten)));
        watch.Restart();
        var path = a.GetShortestPath(new SearchPath(biggestGraph.GetNode(source), biggestGraph.GetNode(target)));
        watch.Stop();
        _output.WriteLine($"a*: {watch.ElapsedMilliseconds}ms");
        Assert.NotNull(path);
        Assert.NotEmpty(path.Nodes);
        Assert.Equal(96, path.Nodes.Length);
    }
}
