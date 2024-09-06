namespace stuff.graph.net;

public static class GraphExtensions
{
    public static IEdge GetEdge(this IGraph graph, long id)
        => graph.Edges.FirstOrDefault(x => x.Key == id).Value ?? throw new KeyNotFoundException($"{id} not found");

    public static INode GetNode(this IGraph graph, long id)
        => graph.Nodes.FirstOrDefault(x => x.Key == id).Value ?? throw new KeyNotFoundException($"{id} not found");

    public static bool AddNode(this IGraph graph, INode node)
        => graph.Nodes.TryAdd(node.Id, node);

    public static bool AddEdge(this IGraph graph, IEdge edge)
    {
        if (!graph.Edges.TryAdd(edge.Id, edge)) return false;
        graph.Nodes[edge.StartNodeId].AddOutgoing(edge.Id);
        graph.Nodes[edge.EndNodeId].AddIncoming(edge.Id);
        return true;
    }
}