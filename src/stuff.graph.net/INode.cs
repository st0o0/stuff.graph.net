using System.Numerics;

namespace stuff.graph.net;

public interface INode
{
    long Id { get; }
    long[] OutgoingEdgeIds { get; }
    long[] IncomingEdgeIds { get; }
    uint RoutingCost { get; }
    Vector3 Location { get; }
    void AddIncoming(long id);
    void AddOutgoing(long id);
}
