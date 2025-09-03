namespace stuff.graph.net;

public static class GraphExtensions
{
    public static (INode? Start, INode? End) GetNodesForEdge(this IGraph graph, long edgeId)
    {
        if (!graph.Edges.TryGetValue(edgeId, out var edge))
        {
            return (null, null);
        }

        graph.Nodes.TryGetValue(edge.StartNodeId, out var start);
        graph.Nodes.TryGetValue(edge.EndNodeId, out var end);

        return (start, end);
    }

    public static IEdge? GetEdgeBetweenNodes(this IGraph graph, INode sourceNode, INode targetNode)
    {
        return graph.Edges.Values
            .FirstOrDefault(e => e.StartNodeId == sourceNode.Id && e.EndNodeId == targetNode.Id);
    }

    public static IEdge GetEdge(this IGraph graph, long id)
        => graph.Edges[id];

    public static INode GetNode(this IGraph graph, long id)
        => graph.Nodes[id];

    public static void AddNode(this IGraph graph, INode node)
        => graph.Nodes.Add(node.Id, node);

    public static void AddEdge(this IGraph graph, IEdge edge)
    {
        graph.Edges.Add(edge.Id, edge);
        _ = edge.GetDirection() switch
        {
            EdgeDirection.OneWay => graph.AddOneWay(edge),
            EdgeDirection.TwoWay => graph.AddTwoWay(edge),
            _ => false
        };
    }

    public static void RemoveEdge(this IGraph graph, IEdge edge)
    {
        graph.Edges.Remove(edge.Id);
        _ = edge.GetDirection() switch
        {
            EdgeDirection.OneWay => graph.RemoveOneWay(edge),
            EdgeDirection.TwoWay => graph.RemoveTwoWay(edge),
            _ => false,
        };
    }

    public static bool RemoveOneWay(this IGraph graph, IEdge edge)
    {
        graph.Nodes[edge.StartNodeId].RemoveOutgoing(edge.Id);
        graph.Nodes[edge.EndNodeId].RemoveIncoming(edge.Id);
        return true;
    }

    private static bool AddOneWay(this IGraph graph, IEdge edge)
    {
        graph.Nodes[edge.StartNodeId].AddOutgoing(edge.Id);
        graph.Nodes[edge.EndNodeId].AddIncoming(edge.Id);
        return true;
    }

    public static bool RemoveTwoWay(this IGraph graph, IEdge edge)
    {
        graph.Nodes[edge.StartNodeId].RemoveIncoming(edge.Id);
        graph.Nodes[edge.EndNodeId].RemoveOutgoing(edge.Id);
        return graph.RemoveOneWay(edge);
    }

    private static bool AddTwoWay(this IGraph graph, IEdge edge)
    {
        graph.Nodes[edge.StartNodeId].AddIncoming(edge.Id);
        graph.Nodes[edge.EndNodeId].AddOutgoing(edge.Id);
        return graph.AddOneWay(edge);
    }
}