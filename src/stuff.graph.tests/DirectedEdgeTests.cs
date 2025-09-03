using stuff.graph.algorithms.net;
using stuff.graph.astar.net;
using stuff.graph.dijkstra.net;
using stuff.graph.net;
using stuff.graph.sortingbarrier.net;

namespace stuff.graph.tests;

public class DirectedEdgeTests
{
    private static Graph BuildDirectedGraph()
    {
        var builder = GraphBuilder.Create();

        var n1 = builder.CreateNode(1, 0, 0, 0);
        var n2 = builder.CreateNode(2, 1, 0, 0);
        var n3 = builder.CreateNode(3, 2, 0, 0);

        // 1 → 2 (OneWay)
        var e12 = builder.CreateDirectedEdge(100, 1, 2, 1, EdgeDirection.OneWay);

        // 2 ↔ 3 (TwoWay)
        var e23 = builder.CreateDirectedEdge(101, 2, 3, 1, EdgeDirection.TwoWay);

        // 3 → 1 (Blocked)
        var e31 = builder.CreateDirectedEdge(102, 3, 1, 5, EdgeDirection.Blocked);

        n1.AddOutgoing(e12.Id);
        n2.AddIncoming(e12.Id);

        n2.AddOutgoing(e23.Id);
        n3.AddIncoming(e23.Id);

        n3.AddOutgoing(e23.Id);
        n2.AddIncoming(e23.Id);

        n3.AddOutgoing(e31.Id);
        n1.AddIncoming(e31.Id);

        return builder.CreateGraph();
    }

    [Fact]
    public void SortingBarrier_Should_Respect_OneWay()
    {
        var g = BuildDirectedGraph();
        var algo = SortingBarrier.Create(new SearchConfig<SortingBarrierSettings>(g, new SortingBarrierSettings()));

        var path = algo.GetShortestPath(new SearchArgs(g.GetNode(1), g.GetNode(2)));

        Assert.NotNull(path);
        Assert.Equal([1, 2], path.Nodes.Select(n => n.Id));

        var pathBack = algo.GetShortestPath(new SearchArgs(g.GetNode(2), g.GetNode(1)));
        Assert.NotNull(pathBack);
        Assert.Empty(pathBack.Nodes);
    }

    [Fact]
    public void SortingBarrier_Should_Allow_TwoWay()
    {
        var g = BuildDirectedGraph();
        var algo = SortingBarrier.Create(new SearchConfig<SortingBarrierSettings>(g, new SortingBarrierSettings()));

        var pathForward = algo.GetShortestPath(new SearchArgs(g.GetNode(2), g.GetNode(3)));
        Assert.NotNull(pathForward);
        Assert.Equal([2, 3], pathForward.Nodes.Select(n => n.Id));

        var pathBackward = algo.GetShortestPath(new SearchArgs(g.GetNode(3), g.GetNode(2)));
        Assert.NotNull(pathBackward);
        Assert.Equal([3, 2], pathBackward.Nodes.Select(n => n.Id));
    }

    [Fact]
    public void SortingBarrier_Should_Block_BlockedEdges()
    {
        var g = BuildDirectedGraph();
        var algo = SortingBarrier.Create(new SortingBarrierConfig(g, new SortingBarrierSettings()));

        var path = algo.GetShortestPath(new SearchArgs(g.GetNode(3), g.GetNode(1)));
        Assert.NotNull(path);
        Assert.Empty(path.Nodes);
    }

    [Fact]
    public void Dijkstra_Should_Respect_OneWay()
    {
        var g = BuildDirectedGraph();
        var algo = Dijkstra.Create(new DijkstraConfig(g));

        var path = algo.GetShortestPath(new SearchArgs(g.GetNode(1), g.GetNode(2)));
        Assert.NotNull(path);
        Assert.Equal([1, 2], path.Nodes.Select(n => n.Id));

        var pathBack = algo.GetShortestPath(new SearchArgs(g.GetNode(2), g.GetNode(1)));
        Assert.NotNull(pathBack);
        Assert.Empty(pathBack.Nodes);
    }

    [Fact]
    public void Dijkstra_Should_Allow_TwoWay()
    {
        var g = BuildDirectedGraph();
        var algo = Dijkstra.Create(new DijkstraConfig(g));

        var pathForward = algo.GetShortestPath(new SearchArgs(g.GetNode(2), g.GetNode(3)));
        Assert.NotNull(pathForward);
        Assert.Equal([2, 3], pathForward.Nodes.Select(n => n.Id));

        var pathBackward = algo.GetShortestPath(new SearchArgs(g.GetNode(3), g.GetNode(2)));
        Assert.NotNull(pathBackward);
        Assert.Equal([3, 2], pathBackward.Nodes.Select(n => n.Id));
    }

    [Fact]
    public void Dijkstra_Should_Block_BlockedEdges()
    {
        var g = BuildDirectedGraph();
        var algo = Dijkstra.Create(new DijkstraConfig(g));

        var path = algo.GetShortestPath(new SearchArgs(g.GetNode(3), g.GetNode(1)));
        Assert.NotNull(path);
        Assert.Empty(path.Nodes);
    }

    [Fact]
    public void AStar_Should_Respect_OneWay()
    {
        var g = BuildDirectedGraph();
        var algo = AStar.Create(new AStarConfig(g, new AStarSettings(Heuristic.Manhatten)));

        var path = algo.GetShortestPath(new SearchArgs(g.GetNode(1), g.GetNode(2)));
        Assert.NotNull(path);
        Assert.Equal([1, 2], path.Nodes.Select(n => n.Id));

        var pathBack = algo.GetShortestPath(new SearchArgs(g.GetNode(2), g.GetNode(1)));
        Assert.NotNull(pathBack);
        Assert.Empty(pathBack.Nodes);
    }

    [Fact]
    public void AStar_Should_Allow_TwoWay()
    {
        var g = BuildDirectedGraph();
        var algo = AStar.Create(new AStarConfig(g, new AStarSettings(Heuristic.Manhatten)));

        var pathForward = algo.GetShortestPath(new SearchArgs(g.GetNode(2), g.GetNode(3)));
        Assert.NotNull(pathForward);
        Assert.Equal([2, 3], pathForward.Nodes.Select(n => n.Id));

        var pathBackward = algo.GetShortestPath(new SearchArgs(g.GetNode(3), g.GetNode(2)));
        Assert.NotNull(pathBackward);
        Assert.Equal([3, 2], pathBackward.Nodes.Select(n => n.Id));
    }

    [Fact]
    public void AStar_Should_Block_BlockedEdges()
    {
        var g = BuildDirectedGraph();
        var algo = AStar.Create(new AStarConfig(g, new AStarSettings(Heuristic.Manhatten)));

        var path = algo.GetShortestPath(new SearchArgs(g.GetNode(3), g.GetNode(1)));
        Assert.Empty(path.Nodes);
    }
}