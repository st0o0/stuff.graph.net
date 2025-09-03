using stuff.graph.erdosrenyi.net;

namespace stuff.graph.tests;

public class ErdosRenyiTests
{
    [Fact]
    public void GenerateGraph_CorrectNumberOfNodesAndReasonableEdgeCount()
    {
        // Arrange
        var generator = ErdosRenyi.Create();
        var numberOfNodes = 10u;
        const double probability = 0.5;

        // Act
        var graph = generator.Generate(new ErdosRenyiArgs(numberOfNodes, probability));

        Assert.Equal((int)numberOfNodes, graph.Nodes.Count);
        Assert.InRange(graph.Edges.Count, 0, (int)numberOfNodes * (numberOfNodes - 1) / 2);
    }

    [Fact]
    public void GenerateGraph_NoEdgesWhenProbabilityIsZero()
    {
        // Arrange
        var generator = ErdosRenyi.Create();
        var numberOfNodes = 10u;
        var probability = 0.0;

        // Act
        var graph = generator.Generate(new ErdosRenyiArgs(numberOfNodes, probability));

        // Assert
        Assert.Equal((int)numberOfNodes, graph.Nodes.Count);
        Assert.Empty(graph.Edges);
    }

    [Fact]
    public void GenerateGraph_AllEdgesWhenProbabilityIsOne()
    {
        // Arrange
        var generator = ErdosRenyi.Create();
        var numberOfNodes = 5u;
        var probability = 1.0;

        // Act
        var graph = generator.Generate(new ErdosRenyiArgs(numberOfNodes, probability));

        // Assert
        Assert.Equal((int)numberOfNodes, graph.Nodes.Count);
        var maxPossibleEdges = numberOfNodes * (numberOfNodes - 1) / 2;
        Assert.Equal((int)maxPossibleEdges, graph.Edges.Count);
    }

    [Fact]
    public void GenerateGraph_ThrowsExceptionForInvalidParameters()
    {
        // Arrange
        var generator = ErdosRenyi.Create();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate(new ErdosRenyiArgs(10u, -0.5)));
        Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate(new ErdosRenyiArgs(10u, 1.5)));
    }
}
