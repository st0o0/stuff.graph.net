using System.Numerics;

namespace stuff.graph.net;

public record Node : INode
{
    public static Node Create(long id, Vector3 location)
        => new() { Id = id, Location = location };

    public static Node Create(long id, float x, float y, float z, uint cost = 0)
        => Create(id, new Vector3(x, y, z)) with { RoutingCost = cost };

    public long Id { get; init; }
    public long[] OutgoingEdgeIds { get; set; } = [];
    public long[] IncomingEdgeIds { get; set; } = [];
    public uint RoutingCost { get; init; } = uint.MinValue;
    public Vector3 Location { get; init; } = Vector3.Zero;
}