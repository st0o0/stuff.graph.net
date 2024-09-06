namespace stuff.graph.net;

public interface IEdge
{
    long Id { get; init; }
    long StartNodeId { get; }
    long EndNodeId { get; }
    uint RoutingCost { get; }
    EdgeDirection GetDirection() => EdgeDirection.TwoWay;
}