using stuff.graph.algorithms.net;
using stuff.graph.net;
using Path = stuff.graph.algorithms.net.Path;

namespace stuff.graph.astar.net;

public class AStar : IAStar
{
    private readonly IGraph _graph;
    private AStarSettings _settings;

    private INodeCostService? _nodeCostService;
    private IEdgeCostService? _edgeCostService;

    public static ISearch<Path, ISearchArgs, AStarSettings> Create(ISearchConfig<AStarSettings> config)
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

    public void Inject(INodeCostService item)
        => _nodeCostService = item;

    public void Inject(IEdgeCostService item)
        => _edgeCostService = item;

    public Path? GetShortestPath(ISearchArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        var sourceNode = args.SourceNode;
        var targetNode = args.TargetNode;
        var openSet = new PriorityQueue<PathNode>();
        var closedSet = new HashSet<long>();

        var startNode = new PathNode(sourceNode.Id, 0, Heuristic(sourceNode, targetNode), null, sourceNode);
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            var currentNode = openSet.Dequeue();
            var node = currentNode.Node;

            if (currentNode.Node.Id == targetNode.Id)
            {
                return ReconstructPath(currentNode);
            }

            closedSet.Add(currentNode.Node.Id);

            foreach (var neighborNode in GetNeighbors(node))
            {
                if (closedSet.Contains(neighborNode.Id))
                {
                    continue;
                }

                var tentativeGScore = currentNode.G +
                                      _nodeCostService.GetValueOrRoutingCost(node) +
                                      _nodeCostService.GetValueOrRoutingCost(neighborNode) +
                                      _edgeCostService.GetValueOrRoutingCost(
                                          _graph.GetEdgeBetweenNodes(node, neighborNode));
                var neighborPathNode = new PathNode(neighborNode.Id, tentativeGScore,
                    Heuristic(neighborNode, targetNode),
                    currentNode,
                    neighborNode);

                if (!openSet.Contains(neighborPathNode, (one, two) => one.Id == two.Id))
                {
                    openSet.Enqueue(neighborPathNode);
                }
                else if (tentativeGScore < neighborPathNode.G)
                {
                    openSet.UpdatePriority(neighborPathNode with { G = tentativeGScore });
                }
            }
        }

        return new Path(sourceNode.Id, targetNode.Id, []);
    }

    private IEnumerable<INode> GetNeighbors(INode node)
    {
        var allEdgesIds = new long[node.OutgoingEdgeIds.Length];
        node.OutgoingEdgeIds.CopyTo(allEdgesIds, 0);

        foreach (var edgeId in allEdgesIds)
        {
            var edge = _graph.Edges[edgeId];
            if (edge.IsBlocked()) continue;
            yield return edge.GetDirection() switch
            {
                EdgeDirection.OneWay => _graph.GetNode(edge.EndNodeId),
                EdgeDirection.TwoWay when edge.StartNodeId == node.Id => _graph.GetNode(edge.EndNodeId),
                EdgeDirection.TwoWay when edge.EndNodeId == node.Id => _graph.GetNode(edge.StartNodeId),
            };
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

    private double Heuristic(INode a, INode b)
        => _settings.Heuristic.Invoke(a, b);
}

public record PathNode(long Id, double G, double H, PathNode? Parent, INode Node) : IComparable<PathNode>
{
    public double F => G + H;
    public int CompareTo(PathNode? other) => F.CompareTo(other?.F);
}