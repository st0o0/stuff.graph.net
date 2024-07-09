namespace stuff.graph.net;

public class DirectedEdge : Edge, IDirectedEdge
{
    public EdgeDirection Direction { get; set; }
    public override EdgeDirection GetDirection() => Direction;
}
