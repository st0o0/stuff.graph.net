using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.dijkstra.net;

public class Dijkstra : IPathfinder<IPathfinderResult, IPathfinderArguments, IPathfinderSettings>
{
    private readonly IGraph _graph;
    private readonly IPathfinderSettings _settings;

    public static IPathfinder<IPathfinderResult, IPathfinderArguments, IPathfinderSettings> Create(IPathfinderConfig<IPathfinderSettings> config)
        => new Dijkstra(config.Graph, config.Settings);

    private Dijkstra(IGraph graph, IPathfinderSettings settings)
    {
        _graph = graph;
        _settings = settings;
    }

    public IPathfinderResult? GetShortestPath(IPathfinderArguments args)
    {
        var source = args.SourceNode;
        var target = args.TargetNode;
        var distances = new Dictionary<long, uint>();
        var priorityQueue = new PriorityQueue<PathNode>();

        foreach (var node in _graph.Nodes.Values)
        {
            distances[node.Id] = uint.MaxValue;
        }

        var pathNode = new PathNode(source.Id, 0, null, source);
        priorityQueue.Enqueue(pathNode);

        while (priorityQueue.Count > 0)
        {
            var currentNode = priorityQueue.Dequeue();
            var node = currentNode.Node;

            if (currentNode.Id == target.Id)
            {
                return ReconstructPath(currentNode);
            }

            foreach (var edgeId in currentNode.Node.OutgoingEdgeIds)
            {
                var edge = _graph.GetEdge(edgeId);
                if(edge is null) continue;
                var neighborNodeId = edge.EndNodeId;

                var newDistance = currentNode.CurrentDistance + edge.RoutingCost + node.RoutingCost;
                if (newDistance < distances[neighborNodeId])
                {
                    distances[neighborNodeId] = newDistance;
                    priorityQueue.Enqueue(new PathNode(neighborNodeId, newDistance, currentNode, _graph.GetNode(neighborNodeId)));
                }
            }
        }

        return null;
    }

    private static Path ReconstructPath(PathNode currentNode)
    {
        List<INode> path = [];
        while (currentNode != null)
        {
            path.Add(currentNode.Node);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        var startNodeId = path[0].Id;
        var endNodeId = path[^1].Id;
        return new Path(startNodeId, endNodeId, path.ToArray());
    }
}

public record Path(long SourceNodeId, long TargetNodeId, INode[] Nodes) : IPathfinderResult;
public record PathNode(long Id, uint CurrentDistance, PathNode? Parent, INode Node) : IComparable<PathNode>
{
    public int CompareTo(PathNode? other) => CurrentDistance.CompareTo(other?.CurrentDistance);
}