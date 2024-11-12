using stuff.graph.net;
using stuff.graph.pagerank.net;

namespace stuff.graph.tests;

public class PageRankTests
{
    [Fact]
    public void PageRank_ShouldComputeCorrectRankings_OnSimpleGraph()
    {
        // Arrange
        var simpleGraph = new Graph();

        var nodeA = Node.Create(0, 0, 0, 0, 0);
        var nodeB = Node.Create(1, 1, 0, 0, 0);
        var nodeC = Node.Create(2, 2, 0, 0, 0);

        simpleGraph.AddNode(nodeA);
        simpleGraph.AddNode(nodeB);
        simpleGraph.AddNode(nodeC);

        var edgeAB = Edge.Create(0, nodeA.Id, nodeB.Id, 1);
        var edgeBC = Edge.Create(1, nodeB.Id, nodeC.Id, 1);

        simpleGraph.AddEdge(edgeAB);
        simpleGraph.AddEdge(edgeBC);

        // Act
        var pageRank = PageRank.Create(new PageRankConfig(simpleGraph, new PageRankSettings(0.85, 0.0000001, 100)));
        var result = pageRank.Calculate();
        var rankings = result.NodeCosts;
        // Assert
        Assert.Equal(3, rankings.Count);
        var totalRank = rankings.Values.Sum();
        Assert.InRange(totalRank, 0.999, 1.3001);
        Assert.True(rankings[2] > 0);
        Assert.True(rankings[1] > 0);
        Assert.True(rankings[0] > 0);
        Assert.True(rankings[1] > rankings[2]);
        Assert.True(rankings[1] > rankings[0]);
    }
}
