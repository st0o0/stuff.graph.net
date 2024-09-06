namespace stuff.graph.net;

public static class EdgeExtensions
{
    public static void SetOneWayOnly(this IDirectedEdge edge)
        => edge.Direction = EdgeDirection.OneWay;
    public static void SetTwoWayOnly(this IDirectedEdge edge)
        => edge.Direction = EdgeDirection.TwoWay;
    public static bool IsOneWayAllowed(this IDirectedEdge edge)
        => edge.Direction == EdgeDirection.OneWay;
    public static bool IsTwoWayAllowed(this IDirectedEdge edge)
        => edge.Direction == EdgeDirection.TwoWay;
}