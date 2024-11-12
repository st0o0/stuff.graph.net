using stuff.graph.wattsstrogatz.net;
using stuff.graph.algorithms.net;

namespace stuff.graph.tests;

public class WattsStrogatzTests
{
    [Fact]
    public void WattsStrogatz_ShouldGenerateCorrectNumberOfNodes()
    {
        // Arrange
        var settings = new WattsStrogatzSettings(0.5);
        var config = new GeneratorConfig<WattsStrogatzSettings>(settings);
        var generator = WattsStrogatz.Create(config);
        var args = new GeneratorArgs(10);

        // Act
        var graph = generator.Generate(args);

        // Assert
        Assert.Equal(10, graph.Nodes.Count);
    }

    [Fact]
    public void WattsStrogatz_ShouldGenerateCorrectNumberOfEdges()
    {
        // Arrange
        var settings = new WattsStrogatzSettings(0.0);
        var config = new GeneratorConfig<WattsStrogatzSettings>(settings);
        var generator = WattsStrogatz.Create(config);
        var args = new GeneratorArgs(10);

        // Act
        var graph = generator.Generate(args);

        // Assert
        var expectedEdges = 10 * 10 / 2;
        Assert.Equal(expectedEdges, graph.Edges.Count);
    }

    [Fact]
    public void WattsStrogatz_ShouldRewireEdges_WithBetaGreaterThanZero()
    {
        // Arrange
        var settings = new WattsStrogatzSettings(1.0);
        var config = new GeneratorConfig<WattsStrogatzSettings>(settings);
        var generator = WattsStrogatz.Create(config);
        var args = new GeneratorArgs(10);

        var originalEdges = new List<(long, long)>();
        for (var i = 0; i < args.NumberOfNodes; i++)
        {
            var neighbor = (i + 1) % args.NumberOfNodes;
            originalEdges.Add((i, neighbor));
        }

        // Act
        var graph = generator.Generate(args);

        // Assert
        bool rewired = false;
        foreach (var edge in graph.Edges.Values)
        {
            var nodeA = edge.StartNodeId;
            var nodeB = edge.EndNodeId;

            var originalNeighbor = (nodeA + 1) % args.NumberOfNodes;

            if (nodeB != originalNeighbor)
            {
                rewired = true;
                break;
            }
        }

        Assert.True(rewired);
    }
}