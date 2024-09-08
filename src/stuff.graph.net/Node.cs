using System.Numerics;

namespace stuff.graph.net;

public record Node : INode
{
    public static Node Create(long id, Vector3 location)
        => new() { Id = id, Location = location };

    public static Node Create(long id, float x, float y, float z) 
        => Create(id, new Vector3(x, y, z));

    public long Id { get; init; }
    public long[] OutgoingEdgeIds { get; private set; } = [];
    public long[] IncomingEdgeIds { get; private set; } = [];
    public uint RoutingCost { get; init; } = uint.MinValue;
    public Vector3 Location { get; init; } = Vector3.Zero;

    public void AddIncoming(long id)
    {
        if (IncomingEdgeIds.Any(x => x == id)) return;
        IncomingEdgeIds = [.. IncomingEdgeIds, id];
    }

    public void AddOutgoing(long id)
    {
        if (OutgoingEdgeIds.Any(x => x == id)) return;
        OutgoingEdgeIds = [.. OutgoingEdgeIds, id];
    }
}