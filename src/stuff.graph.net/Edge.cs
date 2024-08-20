namespace stuff.graph.net;

public class Edge : IEdge
{
    public long Id { get; init; }

    public required INode StartNode { get; init; }

    public required INode EndNode { get; init; }

    public uint RoutingCost { get; init; }

    public float GetLength() => (EndNode.Location - StartNode.Location).Length();
    
    public virtual EdgeDirection GetDirection() => EdgeDirection.TwoWay;
}