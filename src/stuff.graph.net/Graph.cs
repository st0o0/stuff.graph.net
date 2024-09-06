namespace stuff.graph.net;

public record Graph : IGraph
{
    public Guid Id { get; init; }
    public Dictionary<long, IEdge> Edges { get; set; } = [];
    public Dictionary<long, INode> Nodes { get; set; } = [];
}

public interface IGraph
{
    Guid Id { get; init; }
    Dictionary<long, IEdge> Edges { get; set; }
    Dictionary<long, INode> Nodes { get; set; }
}
