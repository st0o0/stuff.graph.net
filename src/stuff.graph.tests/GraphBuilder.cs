using stuff.graph.net;

namespace stuff.graph.tests;

public class GraphBuilder
{
    private GraphSettings _settings;
    private Graph _graph = new();

    public static GraphBuilder Create(GraphSettings settings)
        => new() { _settings = settings };

    public INode CreateNode(long id, float x, float y, float z)
    {
        var node = Node.Create(id, x, y, z) with { RoutingCost = GetNodeCost(uint.MinValue) };
        _graph.AddNode(node);
        return node;
    }

    public IEdge CreateEdge(long id, long startNodeId, long endNodeId)
    {
        var edge = Edge.Create(id, startNodeId, endNodeId) with { RoutingCost = GetEdgeCost(uint.MinValue) };
        _graph.AddEdge(edge);
        return edge;
    }

    public IGraph CreateGraph() => _graph with { Id = Guid.NewGuid() };
    private uint GetEdgeCost(uint routingCost) => _settings.BaseCost + _settings.AdditionalEdgeCost + routingCost;
    private uint GetNodeCost(uint routingCost) => _settings.BaseCost + _settings.AdditionalNodeCost + routingCost;
}

public record GraphSettings(uint BaseCost, uint AdditionalNodeCost, uint AdditionalEdgeCost);