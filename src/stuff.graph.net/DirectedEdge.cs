namespace stuff.graph.net;

public record DirectedEdge : Edge, IDirectedEdge
{
    public EdgeDirection Direction { get; set; }
    public override EdgeDirection GetDirection() => Direction;
}
