namespace stuff.graph.net;

public class Edge : IEdge
{
    public int Id { get; init; }

    public INode StartNode { get; init; }

    public INode EndNode { get; init; }

    public float GetLength() => (EndNode.Location - StartNode.Location).Length();
}