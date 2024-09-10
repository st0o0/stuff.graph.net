using System.Numerics;
using stuff.graph.mwst.net;
using stuff.graph.net;
using stuff.graph.pagerank.net;
using stuff.graph.wcc.net;

namespace stuff.graph.tests;

public class GraphAlgorithmTests
{
    [Fact]
    public void TestKruskalAlgorithm()
    {
        // Arrange
        var graph = CreateTestGraphForMWST();
        var algo = MinimumSpanningTree.Create(new MWSTConfig(graph));
        var result = algo.Find();
        // Assert
        Assert.Equal(3, result.Edges.Length);
        Assert.Equal(1, result.Edges.Count(e => e.Id == 1));
        Assert.Equal(1, result.Edges.Count(e => e.Id == 2));
        Assert.Equal(1, result.Edges.Count(e => e.Id == 3));
    }

    [Fact]
    public void TestWeaklyConnectedComponentsAlgorithm()
    {
        // Arrange
        var graph = CreateTestGraphForWCC();
        var algo = WeaklyConnectedComponents.Create(new WCCConfig(graph));

        // Act
        var result = algo.Find();

        // Assert
        Assert.Equal(2, result.Length);

        var component1 = result.First();
        Assert.Equal(2, component1.Nodes.Count);
        Assert.True(component1.Nodes.Values.Any(node => node.Id == 1), "Die erste Komponente sollte den Knoten 1 enthalten.");
        Assert.True(component1.Nodes.Values.Any(node => node.Id == 2), "Die erste Komponente sollte den Knoten 2 enthalten.");

        var component2 = result.Last();
        Assert.Equal(2, component2.Nodes.Count);
        Assert.True(component2.Nodes.Values.Any(node => node.Id == 3), "Die zweite Komponente sollte den Knoten 3 enthalten.");
        Assert.True(component2.Nodes.Values.Any(node => node.Id == 4), "Die zweite Komponente sollte den Knoten 4 enthalten.");

        var allNodesInComponents = result.SelectMany(c => c.Nodes).Select(n => n.Key).ToHashSet();
        var allNodesInGraph = graph.Nodes.Keys.ToHashSet();
        Assert.Equal(allNodesInGraph, allNodesInComponents);
    }

    [Fact]
    public void TestPageRankAlgorithm()
    {
        // Arrange
        var graph = CreateTestGraphForPageRank();
        var algo = PageRank.Create(new PageRankConfig(graph, new PageRankSettings(0.85, 0.0000001, 100)));
        var result = algo.Calculate();
        // Assert
        Assert.Equal(4, result.NodeCosts.Count);
        Assert.True(result.NodeCosts.Values.All(pr => pr >= 0 && pr <= 1), "Alle PageRank-Werte sollten zwischen 0 und 1 liegen.");
        Assert.True(result.NodeCosts[1] > result.NodeCosts[4], "Der PageRank des Knotens 1 sollte größer sein als der von Knoten 4.");
    }

    private static Graph CreateTestGraphForMWST()
    {
        var builder = GraphBuilder.Create(new GraphSettings(0, 0, 0));
        builder.CreateNode(1, new Vector3(0, 0, 0));
        builder.CreateNode(2, new Vector3(1, 0, 0));
        builder.CreateNode(3, new Vector3(0, 1, 1));
        builder.CreateNode(4, new Vector3(1, 1, 0));
        builder.CreateEdge(1, 1, 2, 1);
        builder.CreateEdge(2, 2, 3, 2);
        builder.CreateEdge(3, 3, 4, 1);
        builder.CreateEdge(4, 1, 4, 5);

        return (Graph)builder.CreateGraph();
    }

    private static Graph CreateTestGraphForWCC()
    {
        var builder = GraphBuilder.Create(new GraphSettings(0, 0, 0));
        builder.CreateNode(1, new Vector3(0, 0, 0));
        builder.CreateNode(2, new Vector3(1, 0, 0));
        builder.CreateNode(3, new Vector3(0, 1, 0));
        builder.CreateNode(4, new Vector3(1, 1, 0));
        builder.CreateEdge(1, 1, 2, 1);
        builder.CreateEdge(2, 3, 4, 1);

        return (Graph)builder.CreateGraph();
    }

    private static Graph CreateTestGraphForPageRank()
    {
        var builder = GraphBuilder.Create(new GraphSettings(0, 0, 0));
        builder.CreateNode(1, new Vector3(0, 0, 0));
        builder.CreateNode(2, new Vector3(1, 0, 0));
        builder.CreateNode(3, new Vector3(0, 1, 0));
        builder.CreateNode(4, new Vector3(1, 1, 0));
        builder.CreateEdge(1, 1, 2, 1);
        builder.CreateEdge(2, 2, 3, 1);
        builder.CreateEdge(3, 3, 1, 1);
        builder.CreateEdge(4, 3, 4, 1);
        return (Graph)builder.CreateGraph();
    }
}
