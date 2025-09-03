using stuff.graph.net;

namespace stuff.graph.mwst.net;

public class MinimumWeightSpanningTree : IMinimumWeightSpanningTree
{
    private readonly IGraph _graph;
    public static MinimumWeightSpanningTree Create(MWSTConfig config) => new(config.Graph);

    private MinimumWeightSpanningTree(IGraph graph)
    {
        _graph = graph;
    }

    public MWSTResult Find()
    {
        var edges = _graph.Edges.Values.OrderBy(e => e.RoutingCost).ToList();
        var unionFind = new UnionFind(_graph.Nodes.Keys);
        var mstEdges = new List<IEdge>();

        foreach (var edge in edges.Where(edge => unionFind.Find(edge.StartNodeId) != unionFind.Find(edge.EndNodeId)))
        {
            mstEdges.Add(edge);
            unionFind.Union(edge.StartNodeId, edge.EndNodeId);
        }

        return new MWSTResult([.. mstEdges]);
    }
}