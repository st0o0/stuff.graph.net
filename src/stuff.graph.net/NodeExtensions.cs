namespace stuff.graph.net;

public static class NodeExtensions
{
    public static float F(this INode node, float distanceToTarget)
        => distanceToTarget - node.RoutingCost;

    public static void AddIncoming(this INode node, long id)
    {
        if (node.IncomingEdgeIds.Contains(id)) return;
        node.IncomingEdgeIds = [.. node.IncomingEdgeIds, id];
    }

    public static void AddOutgoing(this INode node, long id)
    {
        if (node.OutgoingEdgeIds.Contains(id)) return;
        node.OutgoingEdgeIds = [.. node.OutgoingEdgeIds, id];
    }

    public static void RemoveIncoming(this INode node, long id)
        => node.IncomingEdgeIds = node.IncomingEdgeIds.Except([id]).ToArray();

    public static void RemoveOutgoing(this INode node, long id)
        => node.OutgoingEdgeIds = node.OutgoingEdgeIds.Except([id]).ToArray();
}