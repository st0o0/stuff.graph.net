using System.Numerics;

namespace stuff.graph.net;

public interface INode
{
    long Id { get; }
    long[] OutgoingEdgeIds { get; set; }
    long[] IncomingEdgeIds { get; set; }
    uint RoutingCost { get; }
    Vector3 Location { get; }
}