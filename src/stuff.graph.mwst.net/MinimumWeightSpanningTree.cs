using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.mwst.net;

public record MWSTResult(IEdge[] Edges) : IResult;

public record MWSTConfig(IGraph Graph) : IConfig;

public class MinimumSpanningTree : IAlgorithm<MinimumSpanningTree, MWSTConfig>
{
    private readonly IGraph _graph;
    public static MinimumSpanningTree Create(MWSTConfig config) => new(config.Graph);

    private MinimumSpanningTree(IGraph graph)
    {
        _graph = graph;
    }

    public MWSTResult Find()
    {
        var edges = _graph.Edges.Values.OrderBy(e => e.RoutingCost).ToList();
        var unionFind = new UnionFind(_graph.Nodes.Keys);
        var mstEdges = new List<IEdge>();

        foreach (var edge in edges)
        {
            if (unionFind.Find(edge.StartNodeId) != unionFind.Find(edge.EndNodeId))
            {
                mstEdges.Add(edge);
                unionFind.Union(edge.StartNodeId, edge.EndNodeId);
            }
        }

        return new MWSTResult(mstEdges.ToArray());
    }
}

public class UnionFind
{
    private readonly Dictionary<long, long> parent;
    private readonly Dictionary<long, int> rank;

    public UnionFind(IEnumerable<long> elements)
    {
        parent = [];
        rank = [];

        foreach (var element in elements)
        {
            parent[element] = element;
            rank[element] = 0;
        }
    }

    public long Find(long element)
    {
        if (parent[element] != element)
        {
            parent[element] = Find(parent[element]);
        }
        return parent[element];
    }

    public void Union(long element1, long element2)
    {
        long root1 = Find(element1);
        long root2 = Find(element2);

        if (root1 != root2)
        {
            if (rank[root1] > rank[root2])
            {
                parent[root2] = root1;
            }
            else if (rank[root1] < rank[root2])
            {
                parent[root1] = root2;
            }
            else
            {
                parent[root2] = root1;
                rank[root1]++;
            }
        }
    }
}
