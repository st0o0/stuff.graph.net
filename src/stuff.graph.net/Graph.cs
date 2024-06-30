namespace stuff.graph.net;

public class Graph : IGraph
{
    public Guid Id { get; init; }
    public IEdge[] Edges { get; set; }
    public INode[] Nodes { get; set; }
}

public interface IGraph
{
    Guid Id { get; init; }
    IEdge[] Edges { get; set; }
    INode[] Nodes { get; set; }
}
