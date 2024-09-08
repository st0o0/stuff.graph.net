namespace stuff.graph.net;

public static class GraphExtensions
{
    public static IEdge? GetEdge(this IGraph graph, long id)
        => graph.Edges.FirstOrDefault(x => x.Key == id).Value;

    public static INode GetNode(this IGraph graph, long id)
        => graph.Nodes.First(x => x.Key == id).Value;

    public static bool AddNode(this IGraph graph, INode node)
        => graph.Nodes.TryAdd(node.Id, node);

    public static bool AddEdge(this IGraph graph, IEdge edge)
    {
        if (!graph.Edges.TryAdd(edge.Id, edge)) return false;
        return edge.GetDirection() switch
        {
            EdgeDirection.OneWay => graph.OneWay(edge),
            EdgeDirection.TwoWay => graph.TwoWay(edge),
            _ => false
        };
    }

    public static bool OneWay(this IGraph graph, IEdge edge)
    {
        graph.Nodes[edge.StartNodeId].AddOutgoing(edge.Id);
        graph.Nodes[edge.EndNodeId].AddIncoming(edge.Id);
        return true;
    }

    public static bool TwoWay(this IGraph graph, IEdge edge)
    {
        graph.Nodes[edge.StartNodeId].AddIncoming(edge.Id);
        graph.Nodes[edge.EndNodeId].AddOutgoing(edge.Id);
        return graph.OneWay(edge);
    }
}