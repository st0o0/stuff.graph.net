using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.astar.net;

public class AStar : IPathfinder<Path, SearchPath, AStarSettings>, IUpdatable<AStarSettings>
{
    private readonly IGraph _graph;
    private AStarSettings _settings;

    public static IPathfinder<Path, SearchPath, AStarSettings> Create(IConfig<AStarSettings> config)
        => new AStar(config.Graph, config.Settings);

    private AStar(IGraph graph, AStarSettings settings)
    {
        _graph = graph;
        _settings = settings;
    }

    public bool Update(AStarSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        _settings = settings;
        return true;
    }

    public Path? GetShortestPath(SearchPath args)
    {
        ArgumentNullException.ThrowIfNull(args);
        var start = args.SourceNode;
        var goal = args.TargetNode;
        var openSet = new PriorityQueue<PathNode>();
        var closedSet = new HashSet<long>();

        var startNode = new PathNode(start.Id, 0, Heuristic(start, goal), null, start);
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            var currentNode = openSet.Dequeue();
            var node = currentNode.Node;

            if (currentNode.Node.Id == goal.Id)
            {
                return ReconstructPath(currentNode);
            }

            closedSet.Add(currentNode.Node.Id);

            foreach (var neighbor in GetNeighbors(node))
            {
                if (closedSet.Contains(neighbor.Id))
                {
                    continue;
                }

                var tentativeGScore = currentNode.G + Distance(node, neighbor) + node.RoutingCost;
                var neighborNode = new PathNode(neighbor.Id, tentativeGScore, Heuristic(neighbor, goal), currentNode, neighbor);

                if (!openSet.Contains(neighborNode))
                {
                    openSet.Enqueue(neighborNode);
                }
                else if (tentativeGScore < neighborNode.G)
                {
                    openSet.UpdatePriority(neighborNode with { G = tentativeGScore});
                }
            }
        }

        return null;
    }

    private IEnumerable<INode> GetNeighbors(INode node)
    {
        long[] allEdgesIds = new long[node.IncomingEdgeIds.Length + node.OutgoingEdgeIds.Length];
        node.IncomingEdgeIds.CopyTo(allEdgesIds, 0);
        node.OutgoingEdgeIds.CopyTo(allEdgesIds, node.IncomingEdgeIds.Length);

        foreach (var edgeId in allEdgesIds)
        {
            var edge = _graph.GetEdge(edgeId);
            if (edge is null) continue;
            yield return _graph.GetNode(edge.EndNodeId);
        }
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
        return new Path(startNodeId, endNodeId, path.ToArray());
    }

    private static double Distance(INode a, INode b)
        => Math.Sqrt(Math.Pow(a.Location.X - b.Location.X, 2) + Math.Pow(a.Location.Y - b.Location.Y, 2) + Math.Pow(a.Location.Z - b.Location.Z, 2));

    private double Heuristic(INode a, INode b)
        => _settings.Heuristic.Invoke(a, b);
}

public record PathNode(long Id, double G, double H, PathNode? Parent, INode Node) : IComparable<PathNode>
{
    public double F => G + H;
    public int CompareTo(PathNode? other) => F.CompareTo(other?.F);
}