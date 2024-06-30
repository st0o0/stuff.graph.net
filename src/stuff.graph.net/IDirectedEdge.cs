namespace stuff.graph.net;

public interface IDirectedEdge : IEdge
{
    EdgeDirection Direction { get; set; }
}