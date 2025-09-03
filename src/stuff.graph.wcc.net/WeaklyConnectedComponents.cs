using stuff.graph.net;

namespace stuff.graph.wcc.net;

public class WeaklyConnectedComponents : IWeaklyConnectedComponents
{
    private readonly IGraph _graph;
    public static WeaklyConnectedComponents Create(WCCConfig config) => new(config.Graph);

    private WeaklyConnectedComponents(IGraph graph)
    {
        _graph = graph;
    }

    public IGraph[] Find()
    {
        var visited = new HashSet<long>();
        var components = new List<IGraph>();

        foreach (var nodeId in _graph.Nodes.Select(x => x.Key))
        {
            if (visited.Contains(nodeId)) continue;
            var componentGraph = ExploreComponent(nodeId, visited);
            components.Add(componentGraph);
        }

        return [.. components];
    }

    private Graph ExploreComponent(long nodeId, HashSet<long> visited)
    {
        var queue = new Queue<long>();
        queue.Enqueue(nodeId);
        visited.Add(nodeId);

        var componentEdges = new HashSet<IEdge>();
        var componentNodes = new HashSet<INode>();

        while (queue.Count > 0)
        {
            var currentNodeId = queue.Dequeue();
            var currentNode = _graph.Nodes[currentNodeId];

            componentNodes.Add(currentNode);

            var allAdjacentEdges = currentNode.OutgoingEdgeIds.Concat(currentNode.IncomingEdgeIds);

            foreach (var edgeId in allAdjacentEdges)
            {
                if (!_graph.Edges.TryGetValue(edgeId, out var edge)) continue;
                var adjacentNodeId = edge.StartNodeId == currentNodeId ? edge.EndNodeId : edge.StartNodeId;

                componentEdges.Add(edge);

                if (!visited.Add(adjacentNodeId)) continue;
                queue.Enqueue(adjacentNodeId);
            }
        }

        return new Graph
        {
            Id = Guid.NewGuid(),
            Nodes = componentNodes.ToDictionary(n => n.Id, n => n),
            Edges = componentEdges.ToDictionary(e => e.Id, e => e)
        };
    }
}