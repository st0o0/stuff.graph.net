namespace stuff.graph.net;

public interface IEdge
{
    long Id { get; init; }
    INode StartNode { get; }
    INode EndNode { get; }
    uint RoutingCost { get; }
    float GetLength();
    EdgeDirection GetDirection();
}
