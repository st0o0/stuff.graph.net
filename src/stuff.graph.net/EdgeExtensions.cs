namespace stuff.graph.net;

public static class EdgeExtensions
{
    public static void SetOneWayOnly(this IDirectedEdge edge)
        => edge.Direction = EdgeDirection.OneWay;
    public static void SetTwoWayOnly(this IDirectedEdge edge)
        => edge.Direction = EdgeDirection.TwoWay;
    public static bool IsOneWayAllowed(this IEdge edge)
        => edge.GetDirection() == EdgeDirection.OneWay;
    public static bool IsTwoWayAllowed(this IEdge edge)
        => edge.GetDirection() == EdgeDirection.TwoWay;
}