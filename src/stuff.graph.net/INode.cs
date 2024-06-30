using System.Numerics;

namespace stuff.graph.net;

public interface INode
{
    int Id { get; }
    Vector3 Location { get; }
}
