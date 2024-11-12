using System.Numerics;
using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public class GraphBuilder
{
    private GraphSettings _settings;
    private readonly Graph _graph = new();

    public static GraphBuilder Create(GraphSettings settings)
        => new(settings);

    public static GraphBuilder Create()
        => new(new(0, 0, 0));

    private GraphBuilder(GraphSettings settings)
    {
        _settings = settings;
    }

    public int EdgeCount => _graph.Edges.Count;

    public int NodeCount => _graph.Nodes.Count;

    public long[] EdgeIds => _graph.Edges.Keys.ToArray();

    public long[] NodeIds => _graph.Nodes.Keys.ToArray();

    public IReadOnlyDictionary<long, IEdge> Edges => _graph.Edges;

    public IReadOnlyDictionary<long, INode> Nodes => _graph.Nodes;

    public INode CreateNode(long id, Vector3 location, uint cost = 0)
    {
        var node = Node.Create(id, location) with { RoutingCost = GetNodeCost(cost) };
        _graph.AddNode(node);
        return node;
    }

    public IEdge CreateEdge(long id, long startNodeId, long endNodeId, uint cost = 0)
    {
        var edge = Edge.Create(id, startNodeId, endNodeId) with { RoutingCost = GetEdgeCost(cost) };
        _graph.AddEdge(edge);
        return edge;
    }

    public void RemoveEdge(IEdge edge) => _graph.RemoveEdge(edge);

    public Graph CreateGraph(Guid? id = null) => _graph with { Id = id ?? Guid.NewGuid() };
    private uint GetEdgeCost(uint routingCost) => _settings.BaseCost + _settings.AdditionalEdgeCost + routingCost;
    private uint GetNodeCost(uint routingCost) => _settings.BaseCost + _settings.AdditionalNodeCost + routingCost;
}