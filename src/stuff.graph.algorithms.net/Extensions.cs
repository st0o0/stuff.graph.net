using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public static class Extensions
{
    public static uint GetValueOrRoutingCost(this IEdgeCostService? service, IEdge? edge)
        => (service?.Get(edge) ?? edge?.RoutingCost) ?? 0;

    public static uint GetValueOrRoutingCost(this INodeCostService? service, INode? node)
        => (service?.Get(node) ?? node?.RoutingCost) ?? 0;
}