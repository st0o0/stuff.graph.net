using stuff.graph.algorithms.net;
using stuff.graph.astar.net;
using stuff.graph.dijkstra.net;
using stuff.graph.net;

namespace stuff.graph.tests;

public class DijkstraTests
{
    private IGraph SetupGraph()
    {
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));

        // Knoten erstellen
        var nodeA = builder.CreateNode(1, 0, 0, 0);
        var nodeB = builder.CreateNode(2, 1, 0, 0);
        var nodeC = builder.CreateNode(3, 2, 0, 0);
        var nodeD = builder.CreateNode(4, 1, 1, 0);

        // Kanten erstellen
        var edgeAB = builder.CreateEdge(1, 1, 2, 1);
        var edgeBC = builder.CreateEdge(2, 2, 3, 2);
        var edgeAD = builder.CreateEdge(3, 1, 4, 4);
        var edgeDC = builder.CreateEdge(4, 4, 3, 1);

        return builder.CreateGraph();
    }

    [Fact]
    public void TestShortestPath_AtoC()
    {
        // Arrange
        var graph = SetupGraph();
        var pathfinder = Dijkstra.Create(new PathfinderConfig<IPathfinderSettings>(graph, new AStarSettings(Heuristic.DiagonalShortCut)));

        // Act
        var shortestPath = pathfinder.GetShortestPath(new SearchPath(graph.GetNode(1), graph.GetNode(3)));

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
        var pathfinder = Dijkstra.Create(new PathfinderConfig<IPathfinderSettings>(graph, new AStarSettings(Heuristic.DiagonalShortCut)));

        // Act
        var shortestPath = pathfinder.GetShortestPath(new SearchPath(graph.GetNode(1), graph.GetNode(4)));

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
        var pathfinder = Dijkstra.Create(new PathfinderConfig<IPathfinderSettings>(graph, new AStarSettings(Heuristic.DiagonalShortCut)));

        // Entferne die Kanten, die den Zielknoten erreichbar machen
        graph.Edges.Remove(2);  // Entfernt die Kante BC
        graph.Edges.Remove(4);  // Entfernt die Kante DC

        // Act
        var shortestPath = pathfinder.GetShortestPath(new SearchPath(graph.GetNode(1), graph.GetNode(3)));

        // Assert
        Assert.Null(shortestPath);  // Es sollte keinen Pfad geben
    }

    [Fact]
    public void TestSameNode()
    {
        // Arrange
        var graph = SetupGraph();
        var pathfinder = Dijkstra.Create(new PathfinderConfig<IPathfinderSettings>(graph, new AStarSettings(Heuristic.DiagonalShortCut)));

        // Act
        var shortestPath = pathfinder.GetShortestPath(new SearchPath(graph.GetNode(1), graph.GetNode(1)));

        // Assert
        var expectedPath = new List<long> { 1 };
        Assert.NotNull(shortestPath);
        Assert.Equal(expectedPath, shortestPath.Nodes.Select(x => x.Id));
    }
}
