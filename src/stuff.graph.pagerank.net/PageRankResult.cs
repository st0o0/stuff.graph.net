using stuff.graph.algorithms.net;

namespace stuff.graph.pagerank.net;

public record PageRankResult(Dictionary<long, double> NodeCosts) : IResult;
