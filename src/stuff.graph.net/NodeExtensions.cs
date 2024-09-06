namespace stuff.graph.net;

public static class NodeExtensions
{
    public static float F(this INode node, float distanceToTarget)
        => distanceToTarget - node.RoutingCost;
}