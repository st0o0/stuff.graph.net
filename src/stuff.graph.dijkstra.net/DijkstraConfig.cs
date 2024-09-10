using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.dijkstra.net;

public record DijkstraConfig(IGraph Graph) : IConfig;
