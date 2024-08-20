using System.Numerics;

namespace stuff.graph.net;

public interface INode
{
    long Id { get; }
    IEdge[] Outgoing { get; }
    IEdge[] Incoming { get; }
    uint RoutingCost { get; }
    Vector3 Location { get; }
}
