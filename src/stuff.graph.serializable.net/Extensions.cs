using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using stuff.graph.net;

namespace stuff.graph.serializable.net;

public static class Extensions
{
    public static SerializableGraph ToSerializable(this IGraph graph)
        => new(graph.Id, graph.Edges.Select(item => item.Value.ToSerializable()).ToArray(), graph.Nodes.Select(item => item.Value
        .ToSerializable()).ToArray());
    public static SerializableEdge ToSerializable(this IEdge value)
        => value switch
        {
            DirectedEdge directedEdge => directedEdge.ToSerializable(),
            Edge edge => edge.ToSerializable(),
            _ => throw new NotImplementedException(),
        };
    public static SerializableEdge ToSerializable(this Edge edge)
        => new(edge.Id, edge.StartNodeId, edge.EndNodeId, edge.RoutingCost);
    public static SerializableEdge ToSerializable(this DirectedEdge edge)
        => new(edge.Id, edge.StartNodeId, edge.EndNodeId, edge.RoutingCost, edge.Direction);
    public static SerializableNode ToSerializable(this INode node)
        => new(node.Id, node.Location.ToSerializable());
    public static SerializableLocation ToSerializable(this Vector3 vector)
        => new(vector.X, vector.Y, vector.Z);

    public static IGraph To(this SerializableGraph graph)
    {
        var nodes = graph.Nodes.Select(node => node.To()).ToDictionary(x => x.Id)!;
        var edges = graph.Edges.Select(edge => edge.To()).ToDictionary(x => x.Id)!;
        return new Graph() { Id = graph.Id, Edges = edges, Nodes = nodes };
    }

    public static IEdge To(this SerializableEdge edge)
    {
        return edge switch
        {
            { Direction: EdgeDirection.OneWay } => CreateDirectedEdge(edge),
            _ => CreateEdge(edge),
        };
    }

    private static Edge CreateEdge(SerializableEdge edge)
    {
        return new Edge()
        {
            Id = edge.Id,
            StartNodeId = edge.StartNodeId,
            EndNodeId = edge.EndNodeId,
            RoutingCost = edge.RoutingCost,
        };
    }
    private static DirectedEdge CreateDirectedEdge(SerializableEdge edge)
    {
        return new DirectedEdge()
        {
            Id = edge.Id,
            StartNodeId = edge.StartNodeId,
            EndNodeId = edge.EndNodeId,
            RoutingCost = edge.RoutingCost,
            Direction = edge.Direction,
        };
    }

    public static INode To(this SerializableNode node)
        => new Node() { Id = node.Id, Location = node.Location.To() };
        
    public static Vector3 To(this SerializableLocation location)
        => new(location.X, location.Y, location.Z);
}