using System.Numerics;
using stuff.graph.mwst.net;
using stuff.graph.net;
using stuff.graph.pagerank.net;
using stuff.graph.algorithms.net;
using stuff.graph.wcc.net;

namespace stuff.graph.tests;

public class GraphAlgorithmTests
{
    [Fact]
    public void TestMinimumWeightSpanningTreeAlgorithm()
    {
        // Arrange
        var graph = CreateTestGraphForMWST();
        var algo = MinimumWeightSpanningTree.Create(new MWSTConfig(graph));
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
        Assert.Contains(1, component1.Nodes.Keys);
        Assert.Contains(2, component1.Nodes.Keys);

        var component2 = result.Last();
        Assert.Equal(2, component2.Nodes.Count);
        Assert.Contains(3, component2.Nodes.Keys);
        Assert.Contains(4, component2.Nodes.Keys);

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
        Assert.True(result.NodeCosts.Values.All(pr => pr >= 0 && pr <= 1));
        Assert.True(result.NodeCosts[1] > result.NodeCosts[4]);
    }

    private static Graph CreateTestGraphForMWST()
    {
        var builder = GraphBuilder.Create(new stuff.graph.algorithms.net.GraphSettings(0, 0, 0));
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
