using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.pagerank.net;

public record PageRankSettings(double DampingFactor, double Tolerance, int MaxIterations) : ISettings;

public record PageRankConfig(IGraph Graph, PageRankSettings Settings) : IConfig<PageRankSettings>;

public record PageRankResult(Dictionary<long, double> NodeCosts) : IResult;

public class PageRank : IAlgorithm<PageRank, PageRankConfig> 
{
    private readonly IGraph _graph;
    private readonly PageRankSettings _settings;

    public static PageRank Create(PageRankConfig config) => new(config.Graph, config.Settings);

    private PageRank(IGraph graph, PageRankSettings settings)
    {
        _graph = graph;
        _settings = settings;
    }

    public PageRankResult Calculate()
    {
        var nodeCount = _graph.Nodes.Count;
        var pageRank = _graph.Nodes.Keys.ToDictionary(nodeId => nodeId, nodeId => 1.0 / nodeCount);
        var newPageRank = new Dictionary<long, double>(pageRank);

        for (int iteration = 0; iteration < _settings.MaxIterations; iteration++)
        {
            bool converged = true;

            foreach (var node in _graph.Nodes.Values)
            {
                double rankSum = 0.0;

                foreach (var edgeId in node.IncomingEdgeIds)
                {
                    var edge = _graph.Edges[edgeId];
                    var startNode = _graph.Nodes[edge.StartNodeId];
                    rankSum += pageRank[edge.StartNodeId] / startNode.OutgoingEdgeIds.Length;
                }

                newPageRank[node.Id] = (1 - _settings.DampingFactor) / nodeCount + _settings.DampingFactor * rankSum;

                if (Math.Abs(newPageRank[node.Id] - pageRank[node.Id]) > _settings.Tolerance)
                {
                    converged = false;
                }
            }

            var temp = pageRank;
            pageRank = newPageRank;
            newPageRank = temp;

            if (converged)
            {
                break;
            }
        }

        return new PageRankResult(pageRank);
    }
}
