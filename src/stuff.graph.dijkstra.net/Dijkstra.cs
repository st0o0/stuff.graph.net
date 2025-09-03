using stuff.graph.algorithms.net;
using stuff.graph.net;
using Path = stuff.graph.algorithms.net.Path;

namespace stuff.graph.dijkstra.net;

public class Dijkstra : IDijkstra
{
    private readonly IGraph _graph;
    private INodeCostService? _nodeCostService;
    private IEdgeCostService? _edgeCostService;

    public static ISearch<Path, ISearchArgs, ISettings> Create(ISearchConfig config)
        => new Dijkstra(config.Graph);

    private Dijkstra(IGraph graph)
    {
        _graph = graph;
    }

    public void Inject(INodeCostService item)
        => _nodeCostService = item;

    public void Inject(IEdgeCostService item)
        => _edgeCostService = item;

    public Path? GetShortestPath(ISearchArgs args)
    {
        var sourceNode = args.SourceNode;
        var targetNode = args.TargetNode;
        var distances = new Dictionary<long, uint>();
        var priorityQueue = new PriorityQueue<PathNode>();

        try
        {
            foreach (var node in _graph.Nodes.Values)
            {
                distances[node.Id] = uint.MaxValue;
            }

            var pathNode = new PathNode(sourceNode.Id, 0, null, sourceNode);
            priorityQueue.Enqueue(pathNode);

            while (priorityQueue.Count > 0)
            {
                var currentNode = priorityQueue.Dequeue();
                var node = currentNode.Node;

                if (currentNode.Id == targetNode.Id)
                {
                    return ReconstructPath(currentNode);
                }

                foreach (var edgeId in node.OutgoingEdgeIds)
                {
                    var edge = _graph.Edges[edgeId];
                    if (edge.IsBlocked()) continue;
                    var neighborNodeId = edge.GetDirection() switch
                    {
                        EdgeDirection.OneWay => edge.EndNodeId,
                        EdgeDirection.TwoWay when edge.StartNodeId == node.Id => edge.EndNodeId,
                        EdgeDirection.TwoWay when edge.EndNodeId == node.Id => edge.StartNodeId,
                        _ => long.MinValue
                    };

                    var neighborNode = _graph.GetNode(neighborNodeId);
                    var newDistance = currentNode.CurrentDistance +
                                      _nodeCostService.GetValueOrRoutingCost(node) +
                                      _nodeCostService.GetValueOrRoutingCost(neighborNode) +
                                      _edgeCostService.GetValueOrRoutingCost(
                                          _graph.GetEdgeBetweenNodes(node, neighborNode));
                    if (newDistance >= distances[neighborNodeId]) continue;
                    distances[neighborNodeId] = newDistance;
                    priorityQueue.Enqueue(new PathNode(neighborNodeId, newDistance, currentNode,
                        _graph.GetNode(neighborNodeId)));
                }
            }
        }
        catch (Exception)
        {
            return new Path(sourceNode.Id, targetNode.Id, []);
        }

        return new Path(sourceNode.Id, targetNode.Id, []);
    }

    private static Path ReconstructPath(PathNode? currentNode)
    {
        ArgumentNullException.ThrowIfNull(currentNode);
        List<INode> path = [];
        while (currentNode != null)
        {
            path.Add(currentNode.Node);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        var startNodeId = path[0].Id;
        var endNodeId = path[^1].Id;
        return new Path(startNodeId, endNodeId, [.. path]);
    }
}

public record PathNode(long Id, uint CurrentDistance, PathNode? Parent, INode Node) : IComparable<PathNode>
{
    public int CompareTo(PathNode? other) => CurrentDistance.CompareTo(other?.CurrentDistance);
}