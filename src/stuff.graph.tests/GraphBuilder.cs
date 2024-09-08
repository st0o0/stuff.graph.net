using System.Numerics;

namespace stuff.graph.net;

public class GraphBuilder
{
    private GraphSettings _settings;
    private Graph _graph = new();

    public static GraphBuilder Create(GraphSettings settings)
        => new() { _settings = settings };

    public static GraphBuilder Create()
        => new() { _settings = new GraphSettings(0, 0, 0) };

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

    public IGraph CreateGraph(Guid? id = null) => _graph with { Id = id ?? Guid.NewGuid() };
    private uint GetEdgeCost(uint routingCost) => _settings.BaseCost + _settings.AdditionalEdgeCost + routingCost;
    private uint GetNodeCost(uint routingCost) => _settings.BaseCost + _settings.AdditionalNodeCost + routingCost;
}

public record GraphSettings(uint BaseCost, uint AdditionalNodeCost, uint AdditionalEdgeCost);

public static class GraphBuilderExtensions
{
    public static INode CreateNode(this GraphBuilder builder, long id, float x, float y, float z)
        => builder.CreateNode(id, new Vector3(x, y, z));
}