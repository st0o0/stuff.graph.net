using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.wcc.net;

public class WeaklyConnectedComponents : IAlgorithm<WeaklyConnectedComponents, WCCConfig>
{
    private readonly IGraph _graph;
    public static WeaklyConnectedComponents Create(IConfig config) => new(config.Graph);

    private WeaklyConnectedComponents(IGraph graph)
    {
        _graph = graph;
    }

    public IGraph[] Find()
    {
        var visited = new HashSet<long>();
        var components = new List<IGraph>();

        foreach (var node in _graph.Nodes.Values)
        {
            if (!visited.Contains(node.Id))
            {
                var componentGraph = ExploreComponent(node.Id, visited);
                components.Add(componentGraph);
            }
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

            // Füge Knoten zur Komponente hinzu
            if (!componentNodes.Contains(currentNode))
            {
                componentNodes.Add(currentNode);
            }

            var allAdjacentEdges = currentNode.OutgoingEdgeIds.Concat(currentNode.IncomingEdgeIds);

            foreach (var edgeId in allAdjacentEdges)
            {
                if (_graph.Edges.TryGetValue(edgeId, out var edge))
                {
                    var adjacentNodeId = edge.StartNodeId == currentNodeId ? edge.EndNodeId : edge.StartNodeId;

                    // Füge Kante zur Komponente hinzu
                    if (!componentEdges.Contains(edge))
                    {
                        componentEdges.Add(edge);
                    }

                    if (!visited.Contains(adjacentNodeId))
                    {
                        visited.Add(adjacentNodeId);
                        queue.Enqueue(adjacentNodeId);
                    }
                }
            }
        }

        // Erstellen des Graphen für die gefundene Komponente
        return new Graph
        {
            Id = Guid.NewGuid(),
            Nodes = componentNodes.ToDictionary(n => n.Id, n => n),
            Edges = componentEdges.ToDictionary(e => e.Id, e => e)
        };
    }
}