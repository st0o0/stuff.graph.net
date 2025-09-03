using stuff.graph.algorithms.net;
using stuff.graph.dorogovtsevmendes.net;
using stuff.graph.net;

namespace stuff.graph.tests;

public class DorogovtsevMendesTests
{
    [Fact]
    public void GenerateGraph_CorrectNumberOfNodesAndEdges()
    {
        // Arrange
        var generator = DorogovtsevMendes.Create(new GeneratorConfig(new Random()));
        var numberOfNodes = 10u;

        // Act
        IGraph graph = generator.Generate(new GeneratorArgs(numberOfNodes));

        // Assert
        Assert.Equal(numberOfNodes, (uint)graph.Nodes.Count);
        Assert.Equal(10, graph.Edges.Count);

        foreach (var edge in graph.Edges.Values)
        {
            Assert.Contains(edge.StartNodeId, graph.Nodes.Keys);
            Assert.Contains(edge.EndNodeId, graph.Nodes.Keys);
        }
    }

    [Fact]
    public void GenerateGraph_WithThreeNodes_CreatesTriangle()
    {
        // Arrange
        var generator = DorogovtsevMendes.Create();
        var numberOfNodes = 3u;

        // Act
        IGraph graph = generator.Generate(new GeneratorArgs(numberOfNodes));

        // Assert
        Assert.Equal(numberOfNodes, (uint)graph.Nodes.Count);
        Assert.Equal(3, graph.Edges.Count);

        foreach (var node in graph.Nodes.Values)
        {
            Assert.Equal(2, node.OutgoingEdgeIds.Length);
            Assert.Equal(2, node.IncomingEdgeIds.Length);
        }
    }

    [Fact]
    public void GenerateGraph_With15000Nodes()
    {
        var graph = DorogovtsevMendes.Create().Generate(new GeneratorArgs(15000));
        Assert.Equal(15000, graph.Nodes.Count);
        Assert.Equal(15000, graph.Edges.Count);
    }
}
