using System.Numerics;

namespace stuff.graph.net;

public interface IEdge
{
    int Id { get; init; }
    INode StartNode { get; }
    INode EndNode { get; }

    float GetLength();
}
