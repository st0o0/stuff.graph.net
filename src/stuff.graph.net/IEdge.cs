namespace stuff.graph.net;

public interface IEdge
{
    long Id { get; init; }
    long StartNodeId { get; init; }
    long EndNodeId { get; init; }
    uint RoutingCost { get; }
    EdgeDirection GetDirection();
}