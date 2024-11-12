using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.astar.net;

public record AStarConfig(IGraph Graph, AStarSettings Settings) : SearchConfig(Graph), ISearchConfig<AStarSettings>;
