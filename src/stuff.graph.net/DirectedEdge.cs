namespace stuff.graph.net;

public record DirectedEdge : Edge, IDirectedEdge
{
    public static DirectedEdge Create(long id, long startId, long endId, uint cost = 0,
        EdgeDirection direction = EdgeDirection.TwoWay)
        => new() { Id = id, StartNodeId = startId, EndNodeId = endId, RoutingCost = cost, Direction = direction };

    public EdgeDirection Direction { get; set; }
    public override EdgeDirection GetDirection() => Direction;
}