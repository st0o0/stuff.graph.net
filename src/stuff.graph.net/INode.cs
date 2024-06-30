using System.Numerics;

namespace stuff.graph.net;

public interface INode
{
    long Id { get; }
    Vector3 Location { get; }
}
