using System.Numerics;
using stuff.graph.net;
using stuff.graph.serializable.net;

namespace stuff.graph.tests;

public class SerializationTests
{
    [Fact]
    public void FullCircle_Serialization_ShouldWork()
    {
        // Arrange
        var graph = new Graph { Id = Guid.NewGuid() };
        var node1 = new Node { Id = 1, Location = new Vector3(1, 2, 3), RoutingCost = 5 };
        var node2 = new Node { Id = 2, Location = new Vector3(4, 5, 6), RoutingCost = 10 };
        var edge1 = new Edge { Id = 10, StartNodeId = 1, EndNodeId = 2, RoutingCost = 100 };
        var edge2 = new DirectedEdge { Id = 11, StartNodeId = 2, EndNodeId = 1, RoutingCost = 200, Direction = EdgeDirection.OneWay };

        graph.AddNode(node1);
        graph.AddNode(node2);
        graph.AddEdge(edge1);
        graph.AddEdge(edge2);

        // Act
        var serializable = graph.ToSerializable();
        var reconstructed = serializable.To();

        // Assert
        Assert.Equal(graph.Id, reconstructed.Id);
        Assert.Equal(graph.Nodes.Count, reconstructed.Nodes.Count);
        Assert.Equal(graph.Edges.Count, reconstructed.Edges.Count);

        var rNode1 = reconstructed.GetNode(1);
        Assert.Equal(node1.Location, rNode1.Location);

        var rEdge2 = (DirectedEdge)reconstructed.GetEdge(11);
        Assert.Equal(EdgeDirection.OneWay, rEdge2.Direction);
    }

    [Fact]
    public void EdgeToSerializable_ShouldHandleDifferentTypes()
    {
        // Arrange
        var edge = new Edge { Id = 1 };
        var directedEdge = new DirectedEdge { Id = 2, Direction = EdgeDirection.TwoWay };

        // Act
        var sEdge = ((IEdge)edge).ToSerializable();
        var sDirectedEdge = ((IEdge)directedEdge).ToSerializable();

        // Assert
        Assert.Equal(1, sEdge.Id);
        Assert.Equal(2, sDirectedEdge.Id);
        Assert.Equal(EdgeDirection.TwoWay, sDirectedEdge.Direction);
    }
}
