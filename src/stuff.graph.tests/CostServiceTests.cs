using stuff.graph.cost.net;
using stuff.graph.net;
using Moq;

namespace stuff.graph.tests;

public class CostServiceTests
{
    [Fact]
    public void EdgeCostService_Should_AddAndGetCosts()
    {
        // Arrange
        var service = new EdgeCostService();
        service.SetDefaultValue(10);
        var edgeMock = new Mock<IEdge>();
        edgeMock.Setup(e => e.Id).Returns(1);
        edgeMock.Setup(e => e.RoutingCost).Returns(5);

        // Act
        service.TryAddOrUpdate(1, _ => 100, 0);
        var cost = service.Get(edgeMock.Object);

        // Assert
        // base.Get(1, 10) returns 100 (since we added it)
        // 100 + RoutingCost(5) = 105
        Assert.Equal(105u, cost);
    }

    [Fact]
    public void EdgeCostService_Should_ReturnDefaultPlusRoutingCost_WhenNotFound()
    {
        // Arrange
        var service = new EdgeCostService();
        service.SetDefaultValue(10);
        var edgeMock = new Mock<IEdge>();
        edgeMock.Setup(e => e.Id).Returns(1);
        edgeMock.Setup(e => e.RoutingCost).Returns(5);

        // Act
        var cost = service.Get(edgeMock.Object);

        // Assert
        // base.Get(1, 10) returns 10 (default)
        // 10 + RoutingCost(5) = 15
        Assert.Equal(15u, cost);
    }

    [Fact]
    public void EdgeCostService_Should_HandleNullEdge()
    {
        // Arrange
        var service = new EdgeCostService();
        service.SetDefaultValue(10);

        // Act
        var cost = service.Get((IEdge?)null);

        // Assert
        // base.Get(long.MinValue, 10) -> 10
        // 10 + 0 = 10
        Assert.Equal(10u, cost);
    }

    [Fact]
    public void NodeCostService_Should_AddAndGetCosts()
    {
        // Arrange
        var service = new NodeCostService();
        service.SetDefaultValue(20);
        var nodeMock = new Mock<INode>();
        nodeMock.Setup(n => n.Id).Returns(1);
        nodeMock.Setup(n => n.RoutingCost).Returns(5);

        // Act
        service.TryAddOrUpdate(1, _ => 200, 0);
        var cost = service.Get(nodeMock.Object);

        // Assert
        Assert.Equal(205u, cost);
    }

    [Fact]
    public void NodeCostService_Should_HandleNullNode()
    {
        // Arrange
        var service = new NodeCostService();
        service.SetDefaultValue(20);

        // Act
        var cost = service.Get((INode?)null);

        // Assert
        Assert.Equal(20u, cost);
    }

    [Fact]
    public void CostService_TryGetValue_ShouldWork()
    {
        // Arrange
        var service = new EdgeCostService();
        service.TryAddOrUpdate(1, _ => 100, 0);

        // Act
        var found = service.TryGetValue(1, out var value);
        var notFound = service.TryGetValue(2, out var value2);

        // Assert
        Assert.True(found);
        Assert.Equal(100u, value);
        Assert.False(notFound);
        Assert.Equal(0u, value2);
    }
}
