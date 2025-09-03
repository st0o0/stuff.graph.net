using stuff.graph.algorithms.net;
using stuff.graph.net;
using Path = stuff.graph.algorithms.net.Path;

namespace stuff.graph.sortingbarrier.net;

public class SortingBarrier : ISortingBarrier
{
    private readonly IGraph _graph;
    private readonly SortingBarrierSettings _settings;
    private INodeCostService? _nodeCostService;
    private IEdgeCostService? _edgeCostService;

    public static ISearch<ISearchResult, ISearchArgs, SortingBarrierSettings> Create(
        ISearchConfig<SortingBarrierSettings> config)
        => new SortingBarrier(config.Graph, config.Settings);

    private SortingBarrier(IGraph graph, SortingBarrierSettings settings)
    {
        _graph = graph;
        _settings = settings;
    }

    public void Inject(INodeCostService item)
        => _nodeCostService = item;

    public void Inject(IEdgeCostService item)
        => _edgeCostService = item;

    public Path? GetShortestPath(ISearchArgs args)
    {
        var sourceNodeId = args.SourceNode.Id;
        var targetNodeId = args.TargetNode.Id;


        var n = _graph.Nodes.Count;
        var nodeIds = _graph.Nodes.Keys.ToArray();
        var indexOf = nodeIds
            .Select((id, idx) => (id, idx))
            .ToDictionary(x => x.id, x => x.idx);

        if (!indexOf.TryGetValue(sourceNodeId, out var sIdx) || !indexOf.TryGetValue(targetNodeId, out _))
        {
            return new Path(sourceNodeId, targetNodeId, []);
        }

        var dist = Enumerable.Repeat(double.PositiveInfinity, n).ToArray();
        var parent = Enumerable.Repeat(-1L, n).ToArray();
        dist[sIdx] = 0;

        var buckets = new SortedDictionary<long, HashSet<int>>();

        AddToBucket(sIdx);

        var inFrontier = new bool[n];

        while (buckets.Count > 0)
        {
            var (bKey, frontier) = buckets.First();
            buckets.Remove(bKey);

            Array.Clear(inFrontier, 0, inFrontier.Length);
            var current = new Queue<int>(frontier);
            foreach (var v in frontier) inFrontier[v] = true;

            var deferred = new List<int>();

            for (var r = 0; r < _settings.RelaxRounds; r++)
            {
                var levelCount = current.Count;
                if (levelCount == 0) break;

                var next = new Queue<int>();

                for (var i = 0; i < levelCount; i++)
                {
                    var u = current.Dequeue();
                    inFrontier[u] = false;

                    var node = _graph.Nodes[nodeIds[u]];

                    foreach (var edgeId in node.OutgoingEdgeIds)
                    {
                        var edge = _graph.Edges[edgeId];
                        if (edge.IsBlocked()) continue;
                        var vId = edge.GetDirection() switch
                        {
                            EdgeDirection.OneWay => edge.EndNodeId,
                            EdgeDirection.TwoWay when edge.StartNodeId == node.Id => edge.EndNodeId,
                            EdgeDirection.TwoWay when edge.EndNodeId == node.Id => edge.StartNodeId,
                            _ => long.MinValue
                        };

                        var v = indexOf[vId];
                        var nd = dist[u] +
                                 _nodeCostService.GetValueOrRoutingCost(node) +
                                 _nodeCostService.GetValueOrRoutingCost(_graph.GetNode(vId)) +
                                 _edgeCostService.GetValueOrRoutingCost(edge);
                        if (!(nd + 1e-15 < dist[v])) continue;
                        dist[v] = nd;
                        parent[v] = nodeIds[u];

                        var newKey = (long)Math.Floor(nd / _settings.Delta);

                        if (newKey == bKey)
                        {
                            if (inFrontier[v]) continue;
                            next.Enqueue(v);
                            inFrontier[v] = true;
                        }
                        else
                        {
                            deferred.Add(v);
                        }
                    }
                }

                current = next;
            }

            foreach (var v in deferred)
            {
                AddToBucket(v);
            }

            while (current.Count > 0)
            {
                AddToBucket(current.Dequeue());
            }
        }

        var pathNodes = new List<INode>();
        for (var v = targetNodeId; v != -1; v = parent[indexOf[v]])
        {
            pathNodes.Add(_graph.Nodes[v]);
            if (!indexOf.ContainsKey(v)) break;
        }

        pathNodes.Reverse();

        if (pathNodes.Count == 0 || pathNodes[0].Id != sourceNodeId)
        {
            return new Path(sourceNodeId, targetNodeId, []);
        }

        return new Path(sourceNodeId, targetNodeId, pathNodes.ToArray());

        void AddToBucket(int v)
        {
            var key = (long)Math.Floor(dist[v] / _settings.Delta);
            if (!buckets.TryGetValue(key, out var set))
            {
                set = [];
                buckets[key] = set;
            }

            set.Add(v);
        }
    }
}