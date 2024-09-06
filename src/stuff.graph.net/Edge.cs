namespace stuff.graph.net;

public record Edge : IEdge
{
    public static Edge Create(long id, long startId, long endId)
        => new() { Id = id, StartNodeId = startId, EndNodeId = endId };

    public long Id { get; init; }
    public required long StartNodeId { get; init; }
    public required long EndNodeId { get; init; }
    public uint RoutingCost { get; init; }
}