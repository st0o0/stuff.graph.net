using Microsoft.Extensions.DependencyInjection;
using stuff.graph.algorithms.net;
using stuff.graph.astar.net;
using stuff.graph.dependencyinjection.net;
using stuff.graph.dijkstra.net;
using stuff.graph.net;
using stuff.graph.sortingbarrier.net;

namespace stuff.graph.tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddGraphStuff_Should_RegisterGraphProvider()
    {
        // Arrange
        var services = new ServiceCollection();
        var guid = Guid.NewGuid();
        services.AddGraphStuff(_ => new Graph { Id = guid });

        // Act
        var provider = services.BuildServiceProvider();
        var gp = provider.GetRequiredService<IGraphProvider>();

        // Assert
        Assert.NotNull(gp);
        var graph1 = gp.GetGraph();
        var graph2 = gp.GetGraph();
        Assert.Equal(graph1.Id, graph2.Id);
    }

    [Fact]
    public void AddGraphStuff_Should_RegisterShortestPathAlgorithms()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddGraphStuff(_ => new Graph { Id = Guid.NewGuid() });

        // Act
        var provider = services.BuildServiceProvider();

        var aStar = provider.GetService<IAStar>();
        var dijkstra = provider.GetService<IDijkstra>();
        var sortingBarrier = provider.GetService<ISortingBarrier>();

        // Assert
        Assert.NotNull(aStar);
        Assert.NotNull(dijkstra);
        Assert.NotNull(sortingBarrier);
    }

    [Fact]
    public void AddGraphStuff_Should_RegisterEdgeAndNodeCostServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddGraphStuff(_ => new Graph { Id = Guid.NewGuid() });

        // Act
        var provider = services.BuildServiceProvider();

        var edgeCost = provider.GetService<IEdgeCostService>();
        var nodeCost = provider.GetService<INodeCostService>();

        // Assert
        Assert.NotNull(edgeCost);
        Assert.NotNull(nodeCost);
    }

    [Fact]
    public void ScopedGraphProvider_Should_ReturnDifferentGraphsAcrossScopes()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddGraphStuff(_ => new Graph { Id = Guid.NewGuid() });
        var provider = services.BuildServiceProvider();

        // Act
        Graph g1;
        Graph g2;

        using (var scope1 = provider.CreateScope())
        {
            g1 = scope1.ServiceProvider.GetRequiredService<IGraphProvider>().GetGraph();
        }

        using (var scope2 = provider.CreateScope())
        {
            g2 = scope2.ServiceProvider.GetRequiredService<IGraphProvider>().GetGraph();
        }

        // Assert
        Assert.NotEqual(g1.Id, g2.Id);
    }
}