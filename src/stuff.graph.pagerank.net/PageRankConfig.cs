using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.pagerank.net;

public record PageRankConfig(IGraph Graph, PageRankSettings Settings) : IConfig<PageRankSettings>;
