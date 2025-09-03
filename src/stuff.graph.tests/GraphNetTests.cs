using stuff.graph.net;

namespace stuff.graph.tests;

public class GraphNetTests
{
    [Fact]
    public void GraphExtensions_GetNodesForEdge_ShouldWork()
    {
        // Arrange
        var graph = new Graph();
        var node1 = new Node { Id = 1 };
        var node2 = new Node { Id = 2 };
        var edge = new Edge { Id = 10, StartNodeId = 1, EndNodeId = 2 };
        graph.AddNode(node1);
        graph.AddNode(node2);
        graph.AddEdge(edge);

        // Act
        var (start, end) = graph.GetNodesForEdge(10);
        var (startNull, endNull) = graph.GetNodesForEdge(99);

        // Assert
        Assert.Equal(node1, start);
        Assert.Equal(node2, end);
        Assert.Null(startNull);
        Assert.Null(endNull);
    }

    [Fact]
    public void GraphExtensions_GetEdgeBetweenNodes_ShouldWork()
    {
        // Arrange
        var graph = new Graph();
        var node1 = new Node { Id = 1 };
        var node2 = new Node { Id = 2 };
        var edge = new Edge { Id = 10, StartNodeId = 1, EndNodeId = 2 };
        graph.AddNode(node1);
        graph.AddNode(node2);
        graph.AddEdge(edge);

        // Act
        var foundEdge = graph.GetEdgeBetweenNodes(node1, node2);
        var notFoundEdge = graph.GetEdgeBetweenNodes(node2, node1);

        // Assert
        Assert.Equal(edge, foundEdge);
        Assert.Null(notFoundEdge);
    }

    [Fact]
    public void GraphExtensions_RemoveEdge_OneWay_ShouldWork()
    {
        // Arrange
        var graph = new Graph();
        var node1 = new Node { Id = 1 };
        var node2 = new Node { Id = 2 };
        var edge = new DirectedEdge { Id = 10, StartNodeId = 1, EndNodeId = 2, Direction = EdgeDirection.OneWay };
        graph.AddNode(node1);
        graph.AddNode(node2);
        graph.AddEdge(edge);

        // Act
        graph.RemoveEdge(edge);

        // Assert
        Assert.Empty(graph.Edges);
        Assert.Empty(node1.OutgoingEdgeIds);
        Assert.Empty(node2.IncomingEdgeIds);
    }

    [Fact]
    public void GraphExtensions_RemoveEdge_TwoWay_ShouldWork()
    {
        // Arrange
        var graph = new Graph();
        var node1 = new Node { Id = 1 };
        var node2 = new Node { Id = 2 };
        var edge = new DirectedEdge { Id = 10, StartNodeId = 1, EndNodeId = 2, Direction = EdgeDirection.TwoWay };
        graph.AddNode(node1);
        graph.AddNode(node2);
        graph.AddEdge(edge);

        // Act
        graph.RemoveEdge(edge);

        // Assert
        Assert.Empty(graph.Edges);
        Assert.Empty(node1.OutgoingEdgeIds);
        Assert.Empty(node1.IncomingEdgeIds);
        Assert.Empty(node2.OutgoingEdgeIds);
        Assert.Empty(node2.IncomingEdgeIds);
    }

    [Fact]
    public void EdgeExtensions_ShouldWork()
    {
        // Arrange
        var edge = new DirectedEdge { Direction = EdgeDirection.OneWay };

        // Act & Assert
        Assert.True(edge.IsOneWayAllowed());
        Assert.False(edge.IsTwoWayAllowed());
        Assert.False(edge.IsBlocked());

        edge.SetTwoWayOnly();
        Assert.Equal(EdgeDirection.TwoWay, edge.Direction);
        Assert.True(edge.IsTwoWayAllowed());

        edge.SetOneWayOnly();
        Assert.Equal(EdgeDirection.OneWay, edge.Direction);

        var blockedEdge = new DirectedEdge { Direction = EdgeDirection.Blocked };
        Assert.True(blockedEdge.IsBlocked());
    }

    [Fact]
    public void NodeExtensions_F_ShouldWork()
    {
        // Arrange
        var node = new Node { RoutingCost = 10 };

        // Act
        var f = node.F(100);

        // Assert
        Assert.Equal(90f, f);
    }

    [Fact]
    public void NodeExtensions_AddRemove_ShouldBeIdempotent()
    {
        // Arrange
        var node = new Node { Id = 1 };

        // Act
        node.AddIncoming(10);
        node.AddIncoming(10); // Duplicate
        node.AddOutgoing(20);
        node.AddOutgoing(20); // Duplicate

        // Assert
        Assert.Single(node.IncomingEdgeIds);
        Assert.Single(node.OutgoingEdgeIds);

        // Act
        node.RemoveIncoming(10);
        node.RemoveOutgoing(20);

        // Assert
        Assert.Empty(node.IncomingEdgeIds);
        Assert.Empty(node.OutgoingEdgeIds);
    }
}
