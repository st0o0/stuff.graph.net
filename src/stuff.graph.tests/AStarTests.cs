using stuff.graph.algorithms.net;
using stuff.graph.astar.net;

namespace stuff.graph.tests;

public class AstarTests
{
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

        var settings = new AStarSettings(Heuristic.DiagonalShortCut);
        var astar = AStar.Create(new PathfinderConfig<AStarSettings>(graph, settings));

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
        var settings = new AStarSettings(Heuristic.Euclidean);
        var astar = AStar.Create(new PathfinderConfig<AStarSettings>(graph, settings));

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
        var astar = AStar.Create(new PathfinderConfig<AStarSettings>(graph, settings));

        // Act
        var path = astar.GetShortestPath(new SearchPath(startNode, endNode));

        // Assert
        Assert.NotNull(path);
        Assert.Equal(2, path.Nodes.Length);
        Assert.Equal([startNode, endNode], path.Nodes);
    }
}
