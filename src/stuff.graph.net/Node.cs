using System.Numerics;

namespace stuff.graph.net;

public record Node : INode
{
    public long Id { get; init; }
    public required IEdge[] Outgoing { get; init; }
    public required IEdge[] Incoming { get; init; }
    public uint RoutingCost { get; } = uint.MinValue;
    public Vector3 Location { get; init; } = Vector3.Zero;
}
