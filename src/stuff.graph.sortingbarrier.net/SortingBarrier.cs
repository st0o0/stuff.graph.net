using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.sortingbarrier.net;

public class SortingBarrier : ISearch<ISearchResult, ISearchArgs, SortingBarrierSettings>
{
    private IGraph _graph;
    private SortingBarrierSettings _settings;
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

    public ISearchResult? GetShortestPath(ISearchArgs args)
    {
        var sourceId = args.SourceNode.Id;
        var targetId = args.TargetNode.Id;


        var n = _graph.Nodes.Count;
        var nodeIds = _graph.Nodes.Keys.ToArray();
        var indexOf = nodeIds
            .Select((id, idx) => (id, idx))
            .ToDictionary(x => x.id, x => x.idx);

        if (!indexOf.TryGetValue(sourceId, out var sIdx) || !indexOf.TryGetValue(targetId, out _))
        {
            return new Path(sourceId, targetId, []);
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

                    var uNode = _graph.Nodes[nodeIds[u]];

                    foreach (var eid in uNode.OutgoingEdgeIds)
                    {
                        var e = _graph.Edges[eid];
                        var v = indexOf[e.EndNodeId];

                        var nd = dist[u] + e.RoutingCost;
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
        for (var v = targetId; v != -1; v = parent[indexOf[v]])
        {
            pathNodes.Add(_graph.Nodes[v]);
            if (!indexOf.ContainsKey(v)) break;
        }

        pathNodes.Reverse();

        if (pathNodes.Count == 0 || pathNodes.First().Id != sourceId)
        {
            return new Path(sourceId, targetId, []);
        }

        return new Path(sourceId, targetId, pathNodes.ToArray());

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